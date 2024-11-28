
function deleteExercise(exerciseId, workoutProgramId) {
    const updatedExercise = {
        exerciseId: Number(exerciseId),
        workoutProgramId: Number(workoutProgramId)
    };

    fetch("/WorkoutProgram/DeleteExerciseFromWorkoutProgram", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(updatedExercise)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error("Failed to delete exercise");
        }
        return response.json();
    })
    .then(data => {
        console.log("Exercise deleted successfully:", data);
        alert("Exercise deleted successfully!");
    })
    .catch(error => {
        console.error("Error deleting exercise:", error);
        alert("Failed to delete exercise. Please try again.");
    });
};


function addExercise(exerciseId, workoutProgramId) {
    const updatedExercise = {
        exerciseId: Number(exerciseId),
        workoutProgramId: Number(workoutProgramId)
    };

    fetch("/WorkoutProgram/AddExerciseToWorkoutProgram", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(updatedExercise)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to add exercise");
            }
            return response.json();
        })
        .then(data => {
            console.log("Exercise added successfully:", data);
            alert("Exercise added successfully!");
        })
        .catch(error => {
            console.error("Error adding exercise:", error);
            alert("Failed to add exercise. Please try again.");
        });
};



// Handles adding functionality for user to drag or swipe a table row (an exercise within a program) to delete it 
function addSwipeAndDragToDeleteTableRow() {

    const rows = document.querySelectorAll("tbody tr");
    console.log(rows);
    rows.forEach(row => {
        let startX = 0;
        let isSwiping = false;

        const startSwipe = (x) => {
            startX = x;
            isSwiping = true;
        };

        const moveSwipe = (x) => {
            if (isSwiping) {
                const deltaX = x - startX;

                // Only swipe to the right
                if (deltaX > 0) {
                    row.style.transform = `translateX(${deltaX}px)`;
                }
            }
        };

        const endSwipe = (x) => {
            if (isSwiping) {
                const deltaX = x - startX;

                // If swiped far enough, delete row
                if (deltaX > 200) {
                    // Add deleted class for animation
                    row.classList.add("deleted"); 
                    // Remove row after animation
                    setTimeout(() => row.remove(), 300); 

                    //console.log(row.parentElement);
                    const exerciseId = row.getAttribute("data-exercise-id");

                    // Gets the workoutProgramId of the exercise swiped based on the nearest parent card-body containing the id as a dataAttribute
                    const workoutProgramId = row.closest(".card-body").getAttribute("data-workout-program-id");

                    deleteExercise(exerciseId, workoutProgramId);
                } else {
                    // Reset position if not swiped far enough
                    row.style.transform = "translateX(0)";
                }
                isSwiping = false;
            }
        };

        // Touch Events
        row.addEventListener("touchstart", (e) => startSwipe(e.touches[0].clientX));
        row.addEventListener("touchmove", (e) => moveSwipe(e.touches[0].clientX));
        row.addEventListener("touchend", (e) => endSwipe(e.changedTouches[0].clientX));

        // Mouse Events
        let mouseDown = false;

        row.addEventListener("mousedown", (e) => {
            mouseDown = true;
            startSwipe(e.clientX);
        });

        row.addEventListener("mousemove", (e) => {
            if (mouseDown) moveSwipe(e.clientX);
        });

        row.addEventListener("mouseup", (e) => {
            if (mouseDown) {
                mouseDown = false;
                endSwipe(e.clientX);
            }
        });

        row.addEventListener("mouseleave", (e) => {
            // Cancel swipe if mouse leaves the row
            if (mouseDown) {
                e.preventDefault();
                e.dropEffect = "none";
                //mouseDown = false;
                //row.style.transform = "translateX(0)";
            }
        });
    });
};


function filterWorkoutPrograms() {
    const searchBox = document.querySelector("#searchBox");
    const programCards = document.querySelectorAll(".card");

    searchBox.addEventListener("input", function () {
        const searchTerm = searchBox.value.toLowerCase();

        programCards.forEach(card => {
            const programName = card.querySelector(".card-title").textContent.toLowerCase();
            const programDescription = card.querySelector("#programContainer").textContent.toLowerCase();

            // Show or hide the card based on search match
            if (programName.includes(searchTerm) || programDescription.includes(searchTerm)) {
                card.style.display = "block";
            } else {
                card.style.display = "none";
            }
        });
    });
}


// Adds/removes the class responsible for hiding/displaying exercises of a program retrieved via the fetch request 
function toggleExercisesHidden(exercisesContainer, expandableDetails) {
    if (exercisesContainer.classList.contains("collapse")) {
        exercisesContainer.classList.remove("collapse");
        expandableDetails.classList.remove("hidden");
        //toggleButton.textContent = "Hide Exercises";
    } else {
        exercisesContainer.classList.add("collapse");
        expandableDetails.classList.add("hidden");
        //toggleButton.textContent = "Show Exercises";
    }
}

document.addEventListener('DOMContentLoaded', function () {
    // Initializes the JS responsible for handling the filtering of workout programs in the search box
    filterWorkoutPrograms();

    // The workoutProgramId is attached to each card body as a custom data attribute in the .cshtml
    const workoutPrograms = document.querySelectorAll(".card-body");
    // Maintains all the workoutProgramIds already fetched to reduce unnecassery queries to db
    const fetchedPrograms = new Set();

    console.log(fetchedPrograms);
    // Iterate through each program and add an event listener which is used to lazy-load exercises related to a workout program
    workoutPrograms.forEach(program => {
        program.addEventListener("click", function (e) {
            const workoutProgramId = program.dataset.workoutProgramId;
            const exercisesContainer = program.querySelector(".exercisesContainer");
            const expandableDetails = program.querySelector(".expandable-details");


            // Prevents re-fetching of exercises in a program if already retrieved
            if (!fetchedPrograms.has(workoutProgramId)) {
                console.log("fetching programs...")
                // Send a request to fetch exercises associated with the workout program ID
                fetch("/WorkoutProgram/ListWorkoutExercises?workoutProgramId=" + workoutProgramId)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error("Failed to fetch exercises");
                        }
                        return response.json();
                    })
                    .then(exercises => {
                        console.log("Fetched exercises:", exercises);

                        // Select the exercises container within the clicked card body
                        //const exercisesContainer = program.querySelector(".exercisesContainer");

                        // Clear any existing content to avoid duplication
                        exercisesContainer.textContent = "";

                        // Check if there are exercises to display
                        if (exercises.length === 0) {
                            exercisesContainer.textContent = "No exercises found for this program.";
                            return;
                        }


                        // Create a table to display the fetched list of exercises
                        const table = document.createElement("table");
                        table.className = "table table-striped";


                        // Create and append the table header
                        const thead = document.createElement("thead");
                        const headerRow = document.createElement("tr");

                        const headers = ["Exercise", "Category", "Description"];
                        headers.forEach(headerText => {
                            const th = document.createElement("th");
                            th.textContent = headerText;
                            headerRow.appendChild(th);
                        });

                        thead.appendChild(headerRow);
                        table.appendChild(thead);


                        const tbody = document.createElement("tbody");

                        // Create and append the new table elements for each exercise
                        exercises.forEach(exercise => {
                            const row = document.createElement("tr");

                            // Create and append table cells for each property
                            const exerciseNameCell = document.createElement("td");
                            exerciseNameCell.textContent = exercise.exerciseName;
                            row.appendChild(exerciseNameCell);

                            const muscleGroupCell = document.createElement("td");
                            muscleGroupCell.textContent = exercise.muscleGroup;
                            row.appendChild(muscleGroupCell);

                            const descriptionCell = document.createElement("td");
                            descriptionCell.textContent = exercise.exerciseDescription;
                            row.appendChild(descriptionCell);

                            tbody.appendChild(row);

                            // Assigns a custom data attribute to the generated row of its exerciseId in the db
                            row.dataset.exerciseId = exercise.exerciseId;
                            row.dataset.workoutProgramId = exercise.workoutProgramId;
                        });

                        table.appendChild(tbody);
                        exercisesContainer.appendChild(table);

                        // Adds an event listener to each row to determine which row has been clicked and if it has been dragged/swiped to initiate deletion
                        addSwipeAndDragToDeleteTableRow();


                        // Add the button below the table
                        const buttonContainer = document.createElement("div");
                        buttonContainer.className = "mt-3"; // Add some spacing above the button

                        const addButton = document.createElement("button");
                        addButton.textContent = " Add New Exercise"; // Space added for the icon
                        addButton.className = "btn btn-primary w-100 d-flex align-items-center justify-content-center";

                        // Add the + icon
                        const plusIcon = document.createElement("i");
                        plusIcon.className = "fas fa-plus me-2"; // Font Awesome icon with margin on the right
                        addButton.prepend(plusIcon);

                        addButton.addEventListener("click", () => {
                            console.log("Add New Exercise button clicked for program ID:", workoutProgramId);
                            // Add your logic here for adding a new exercise
                        });

                        buttonContainer.appendChild(addButton);
                        exercisesContainer.appendChild(buttonContainer);


                        // If fetch request successful, adds its ID to the set to prevent the query from re-running
                        fetchedPrograms.add(workoutProgramId);
                    })
                    .catch(error => {
                        console.error("Error fetching exercises:", error);

                        // Show an error message if fetch fails
                        const exercisesContainer = program.querySelector(".exercisesContainer");
                        exercisesContainer.textContent = "Error loading exercises. Please try again.";
                    });
            } else {
                console.log("Workout Program ID ${workoutProgramId} already fetched, aborting query...");

                // Checks if the user clicked inside of the exercise container, if so, prevents the workout program card from being hidden/collapsed
                if (exercisesContainer && exercisesContainer.contains(e.target)) {
                    console.log("Clicked inside exercisesContainer");
                    

                } else {
                    console.log("Clicked outside exercisesContainer");
                    toggleExercisesHidden(exercisesContainer, expandableDetails);

                }

            }
            
        })
    })
});