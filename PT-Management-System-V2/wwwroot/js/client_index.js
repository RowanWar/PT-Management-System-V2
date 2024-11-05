//function toggleDetails(button) {
//    button.classList.toggle('.hidden-by-default');
//    //elements = document.querySelectorAll(.)
//    //// Toggle visibility of the next sibling element (extra-details div)
//    //const extraDetails = button.nextElementSibling;

//    //if (extraDetails.style.display === "none") {
//    //    extraDetails.style.display = "block";
//    //    button.textContent = "Show Less Details";
//    //} else {
//    //    extraDetails.style.display = "none";
//    //    button.textContent = "Show More Details";
//    //}

//}


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

//document.addEventListener("DOMContentLoaded", function () {

//    const clientBody = document.querySelectorAll(".client-body");

//    const clientContainer = this.closest(".client-container"); 
//    const extraDetailsElements = clientContainer.querySelectorAll(".extra-details");


//    clientBody.forEach(elem => {
//        elem.addEventListener("click", function (button) {
//            // Toggles a class which sets display to none
//            extraDetailsElements.forEach(elem => {
//                elem.classList.toggle("hidden-by-default");
//            });
//        })
//    })
//});
