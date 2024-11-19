function updateWorkoutDate(workoutScheduleId, dayOfWeek, muscleGroup) {
    const payload = {
        workoutScheduleId: Number(workoutScheduleId),
        dayOfWeek: Number(dayOfWeek),
        muscleGroup: muscleGroup
    };

    //console.log(payload)

    // Send the data to the controller
    fetch('/Calendar/UpdateWorkoutProgram', {
        method: 'POST', 
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to update the workout schedule.');
            }

            console.log('Workout schedule updated successfully:', response);
            showNotification('Program schedule updated!', true);
        })
        .catch(error => {
            console.error('Error updating workout schedule:', error);
            showNotification('Error updating program!', false);
        });
};



document.addEventListener('DOMContentLoaded', function () {

    const submitButton = document.querySelector("#submitButton");
    const cancelButton = document.querySelector("#cancelButton");





    // FullCalendar component
    var calendarEl = document.getElementById("calendar");
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: "dayGridWeek",
        headerToolbar: false,                    // Hide the default header (optional)
        allDaySlot: false,                       // Remove "all-day" section
        weekends: true,                          // Enable weekends (Sat, Sun)
        dayHeaders: true,                        // Show only day names without dates
        dayHeaderFormat: { weekday: "long" },    // Removes the dates from the header so it only displays days of week i.e. mon, tue
        firstDay: 1,
        contentHeight: "auto",
        events: "/Calendar/GetDaysOfWeek",
        editable: true,                           // Allow drag-and-drop for testing
        droppable: false,                          // Disable external dragging if unused
        displayEventTime: false,      // Hide time for events
        dateClick: null,              // Disable date click


        eventDrop: function (info) {
            // Handle workout re-scheduling
            // Uses Prototype() to fetch the day of the week (dow) 
            let dayOfWeek = info.event.start.getDay();
            let muscleGroup = info.event.title;
            let workoutScheduleId = info.event.id;

            //console.log(day);
            console.log(workoutScheduleId, dayOfWeek, muscleGroup);
            updateWorkoutDate(workoutScheduleId, dayOfWeek, muscleGroup);
        }
    });

    calendar.render();




    const dropdown = document.getElementById('programDropdown');
    const originalValue = dropdown.textContent; // Store the original value for reset
    console.log(originalValue);

    // Show buttons when a new program is selected
    dropdown.addEventListener("change", function () {
        if (dropdown.value !== originalValue) {
            buttonContainer.style.display = "block";
        } else {
            buttonContainer.style.display = "none";
        }
    });

    // Handle cancel button click
    cancelButton.addEventListener("click", function () {
        dropdown.value = originalValue; // Reset to the original value
        buttonContainer.style.display = "none"; // Hide buttons
    });
});



function showNotification(message, isSuccess = true) {
    const notification = document.getElementById('notification');

    // Set the message and apply the appropriate style
    notification.textContent = message;
    notification.className = isSuccess ? 'success show' : 'error show';

    // Automatically hide the notification after 3 seconds
    setTimeout(() => {
        notification.className = 'hidden';
    }, 2000);
};




// Using lazy-loading to minimise page-loading duration. Currently, this performs a query to the db every time the user clicks the dropdown btn. 
// This should be moved to save query to SESSION STORAGE, and deleted after to save db trips
document.querySelector(".loadProgramsButton").addEventListener("click", function () {
    event.preventDefault();

    programDropdown = document.querySelector("#programDropdown");
    // Check if dropdown is exists (is truthy)
    // If program data has already been fetched (options more than 1 exist), prevents running the fetch req again.
    if (programDropdown) {
        if (programDropdown.options.length > 1) return;
    };

    fetch("/workoutprogram/ListWorkoutPrograms") // Adjust controller name if needed
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to fetch programs");
            }
            return response.json();
        })
        .then(programs => {
            console.log("Ran fetch req for programs");
            const dropdown = document.getElementById("programDropdown");
            let currentValue = dropdown.value;
            const originalValue = dropdown.textContent; // Store the original value for reset
            

            // Clear existing options except the default (original) value
            //dropdown.textContent = currentValue;
            while (dropdown.firstChild) {
                dropdown.removeChild(dropdown.firstChild);
            }



            // Populate dropdown with fetched data
            programs.forEach(program => {
                const option = document.createElement("option");
                // The unique ID of the program
                option.value = program.workoutProgramId; 
                option.textContent = program.programName;

                dropdown.appendChild(option);
            });


            // Replace selected program with the new selected value
            if (currentValue == "Default") {
                console.log('Is empty');
                //dropdown.textContent = originalValue;
            }
            dropdown.value = currentValue;
        })
        .catch(error => {
            console.error("Error loading programs:", error);
            alert("Failed to load programs. Please try again.");
        });
});





