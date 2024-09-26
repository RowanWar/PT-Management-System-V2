
let modal = document.getElementById("myModal");
let btn = document.querySelectorAll(".getResultButton");
let span = document.querySelector(".close");

function buttonClicked(ReportId) {
    console.log('Report ID: ' + ReportId);

    fetch('/Client/ViewImage?ReportId=' + ReportId)
        .then(response => response.json())
        .then(data => {
            // Causes the modal to pop-up upon SQL query returning succesfully
            modal.style.display = "block";

            // Tells JS to expect and parse as a Json obj.
            var jsonArr = JSON.parse(data);


            // Iterates through each returned image to unpack its unique ID and display it in the modal.
            for (let i = 0; i < jsonArr.length; i++) {
                console.log(i);
                console.log(jsonArr[i]["ImageFilePath"]);

                let fileId = jsonArr[i]["ImageFilePath"];

                let generateImage = document.createElement("img");
                // first slash in "/images/" dictates its an absolute path and not relative
                generateImage.src = "/images/" + fileId;

                document.querySelector('.images-div').appendChild(generateImage);
            }
            
        })
        .catch(error => {
            console.error('Error fetching data: ', error);
        });
}


span.onclick = function () {
    modal.style.display = "none";

    // Grabs the parent node inside of the modal-content
    let createdImage = document.querySelector('.images-div');

    while (createdImage.firstChild) {
        createdImage.removeChild(createdImage.firstChild);
    }
}
