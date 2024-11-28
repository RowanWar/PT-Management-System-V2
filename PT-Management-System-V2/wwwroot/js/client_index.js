
function toggleDetails(button) {
    const clientContainer = button.closest(".client-container"); 

    // Uses a static class name on all non-default (hidden) data within a client card so the toggle functions
    const extraDetailsElements = clientContainer.querySelectorAll(".extra-details");

    // Toggles a class which sets display to none
    extraDetailsElements.forEach(elem => {
        elem.classList.toggle("hidden-by-default");
    });

    // Changes the button text
    if (extraDetailsElements[0].classList.contains("hidden-by-default")) {
        button.textContent = "Show More Details";
    } else {
        button.textContent = "Show Less Details";
    }

}


// Adds an event listener to all client-bodies (clients) that have been generated dynamically on the page 
document.addEventListener("DOMContentLoaded", function () {
    const clientBodies = document.querySelectorAll(".client-body");

    clientBodies.forEach(clientBody => {
        clientBody.addEventListener("click", function () {
            // If another modal is already open when the user clicks onto a client, closes it.
            document.querySelectorAll(".client-modal").forEach(modal => {
                modal.style.display = "none";
            });

            // Display the modal within the client-body that has been clicked
            const clientModal = this.querySelector(".client-modal");
            clientModal.style.display = "block";
        });
    });
});

// Handles closing the modal
function closeModal(button) {

    event.stopPropagation();
    // Grab the parent modal belonging to the clientBody (client)
    let modalElement = button.closest(".client-modal");
    modalElement.style.display = "none";
}