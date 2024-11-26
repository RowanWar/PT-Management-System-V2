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
                        });

                        table.appendChild(tbody);
                        exercisesContainer.appendChild(table);

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
                //toggleExercisesHidden(exercisesContainer, expandableDetails);
                //console.log(e);
            }
            
        })
    })
    // Initiate an async fetch req to the controller to retrieve the list of exercises associated with each workout program
});