



//async function loadMoreCheckIns() {
//    fetch('/YourCoach/CoachCheckIns?page=1&pageSize=4')
//        .then(response => response.json())
//        .then(data => {
//            checkInContainer = document.querySelector(".checkInsContainer");

//            data.forEach(checkIn => {
//                const checkInElement = document.createElement('div');
//                checkInElement.className = "check-in-item";

//                checkInContainer.content('Hello');
//                checkInContainer.appendChild(checkInElement);

//            });
//        })}

//document.addEventListener("DOMContentLoaded", function () {
//    loadMoreCheckIns();
//});



let currentPage = 1;
const pageSize = 4;

async function loadMoreCheckIns() {
    try {
        console.log(currentPage);
        const response = await fetch(`/YourCoach/CoachCheckIns?page=${currentPage}`);

        if (!response.ok) {
            throw new Error("Network response error: " + response.statusText);
        }

        // Stores the JSON array returned
        const checkIns = await response.json();


        if (checkIns.length === 0) {
            document.getElementById('loadMoreBtn').style.display = 'none';
            return;
        }


        const container = document.getElementById('checkInsContainer');

        checkIns.forEach(checkIn => {
            // Create a div for each check-in item
            const checkInElement = document.createElement('div');
            checkInElement.className = "check-in-item";

            // Create and populate Date element
            const dateElement = document.createElement('p');
            dateElement.textContent = `Date: ${new Date(checkIn.checkInDate).toLocaleDateString()}`;
            checkInElement.appendChild(dateElement);

            // Create and populate Weight element
            const weightElement = document.createElement('p');
            weightElement.textContent = `Weight: ${checkIn.checkInWeight}`;
            checkInElement.appendChild(weightElement);

            // Create and populate Notes element
            const notesElement = document.createElement('p');
            notesElement.textContent = `Notes: ${checkIn.notes}`;
            checkInElement.appendChild(notesElement);

            const separator = document.createElement('hr');
            checkInElement.appendChild(separator);

            // Append the complete check-in item to the container
            container.appendChild(checkInElement);
        })

        currentPage++;
            
    } catch (error) {
        console.error("Error loading check-ins:", error);
    }
}


document.addEventListener("DOMContentLoaded", function () {
    loadMoreCheckIns();
});

