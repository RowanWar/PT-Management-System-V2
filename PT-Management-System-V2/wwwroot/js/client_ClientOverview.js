function updateWorkoutDate(workoutScheduleId, dayOfWeek, muscleGroup) {
    const payload = {
        workoutScheduleId: Number(workoutScheduleId),
        dayOfWeek: Number(dayOfWeek),
        muscleGroup: muscleGroup
    };


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

// Updates the full calendar displayed for daysOfWeek based on the new workout program selected by the coach
function updateCalendar(newWorkoutProgramId, calendar) {
    calendar.removeAllEvents();

    // Add the new array of days of week from the new program
    fetch("/Calendar/GetDaysOfWeek?workoutProgramId=" + newWorkoutProgramId)
        .then(response => response.json())
        .then(dayOfWeek => {
            calendar.addEventSource(dayOfWeek);
        })
        .catch(error => {
            console.error("Error fetching updated events: ", error);
        });
}


function handleSubmitButtonPostReq(event, calendar) {
    event.preventDefault(); // Stop the form from redirecting
    const form = document.querySelector("#programForm");
    const formData = new FormData(form); // Collect the form data
    const payload = {
        workoutProgramId: Number(formData.get('workoutProgramId')),
        clientId: Number(formData.get('clientId'))
    };

    console.log(payload);

    fetch(form.action, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to update the workout program.");
            }
            return response.json();
        })
        .then(data => {
            console.log("Workout program updated successfully:", data);
            showNotification("Workout program updated!", true);
            // This function refreshes the days of the week within the calendar based on new programs id
            updateCalendar(payload.workoutProgramId, calendar);
            buttonContainer.style.display = "none";
        })
        .catch(error => {
            console.error("Error updating workout program:", error);
            showNotification("Error updating workout program!", false);
        });
    
            
};

document.addEventListener('DOMContentLoaded', function () {

    // Interrupts the submit btn on the form to prevent redirect and handle updating workout programs via AJAX
    document.querySelector('#programForm').addEventListener('submit', function (event) {
        handleSubmitButtonPostReq(event, calendar);
    });


    const submitButton = document.querySelector("#submitButton");
    const cancelButton = document.querySelector("#cancelButton");

    const dropdown = document.getElementById('programDropdown');
    const originalValue = dropdown.textContent; // Store the original value for reset
    console.log(originalValue);

    // The clients current program id is assigned to the default placeholder value via @Model in the .cshtml on model > view page load.
    let currentWorkoutProgramId = dropdown.value;


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
        // Utilises the workoutProgramId passed in from the model and fetched by the data passed on page load via controller
        events: "/Calendar/GetDaysOfWeek?workoutProgramId=" + currentWorkoutProgramId,
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

            updateWorkoutDate(workoutScheduleId, dayOfWeek, muscleGroup);
        }
    });

    calendar.render();






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

    
    notification.textContent = message;
    notification.className = isSuccess ? "success show" : "error show";

    // Automatically hide the notification after 2 seconds
    setTimeout(() => {
        notification.className = "hidden";
    }, 2000);
};




// Using lazy-loading to minimise page-loading duration. Currently, this performs a query to the db every time the user clicks the dropdown btn. 
// This should be moved to save query to SESSION STORAGE, and deleted after to save db trips
document.querySelector(".loadProgramsButton").addEventListener("click", function (event) {
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





