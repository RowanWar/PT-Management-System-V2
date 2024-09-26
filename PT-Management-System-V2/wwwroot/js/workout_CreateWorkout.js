let minute = 00;
let second = 00;
let count = 00;

// Static for now, this should be fetched later upon user login.
const UserId = 1;
let WorkoutId = null;


// This needs to be made an individual function, and then simply called when content loaded. This seperated function can then be called AFTER the cachedWorkout is deleted upon a new set being created.
document.addEventListener("DOMContentLoaded", function () {

    timer = true;
    stopWatch();

    // Checks if the users workout is already cached in localstorage. If key does not exist, returns null and does not initiate the query.
    if (localStorage.getItem("cachedWorkout") == null) {
        console.log('No cached workout found, fetching from database now...');
        CheckForActiveWorkout();

    }

    else {
        console.log("Cached workout found.")
        let cachedWorkout = localStorage.getItem("cachedWorkout");
        let parsedCachedWorkout = JSON.parse(cachedWorkout);

        //let cachedWorkoutId = localStorage.getItem("workoutId");
        //let parsedCachedWorkoutId = JSON.parse(cachedWorkoutId);

        loadActiveWorkoutExercises(parsedCachedWorkout);
    }
});



function stopWatch() {
    if (timer) {
        count++;

        if (count == 100) {
            second++;
            count = 0;
        }

        if (second == 60) {
            minute++;
            second = 0;
        }

        // if (minute == 60) {
        //     hour++;
        //     minute = 0;
        //     second = 0;
        // }

        // let hrString = hour;
        let minString = minute;
        let secString = second;
        let countString = count;

        // if (hour < 10) {
        //     hrString = "0" + hrString;
        // }

        if (minute < 10) {
            minString = "0" + minString;
        }

        if (second < 10) {
            secString = "0" + secString;
        }

        // if (count < 10) {
        //     countString = "0" + countString;
        // }

        // document.getElementById('hr').innerHTML = hrString;
        document.getElementById('min').innerHTML = minString;
        document.getElementById('sec').innerHTML = secString;
        document.getElementById('count');
        setTimeout(stopWatch, 10);
    }
};



let modal = document.getElementById("myModal");
let btn = document.querySelector("#addExerciseBtn");
let span = document.querySelector(".close");


function CheckForActiveWorkout() {
    fetch('/Workout/CheckForActiveWorkout?UserId=' + UserId)
        .then(response => response.json())
        .then(data => {
            let activeWorkoutObj = JSON.parse(data);
            WorkoutId = activeWorkoutObj[0].WorkoutId;

            localStorage.setItem("workoutId", WorkoutId);
            // If the first API lookup does not return a valid WorkoutId, the second API lookup which pulls the users active workout does not initiate.
            if (WorkoutId != null) {
                queryActiveWorkoutExercises();

            }
        })
        .catch(error => {
            console.log('Error occurred:', error);
        });
}

function submitExercises() {
    let cachedWorkoutId = localStorage.getItem("workoutId");
    let WorkoutId = JSON.parse(cachedWorkoutId);
    console.log(WorkoutId);
    //Converts the GLOBAL string array (required to be used by .push and .splice in activeRows()) to an integer so it can be parsed correctly by the backend, which only accepts <int> data type.
    let ExerciseIdsIntArray = selectedExerciseIds.map(item => Number(item));
/*    let WorkoutId = 201;*/
    console.log(ExerciseIdsIntArray);
    fetch('/Workout/InsertExercises', {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            ExerciseIds: ExerciseIdsIntArray, 
            WorkoutId: WorkoutId
        })
    })
        .then(response => response.json())
        .then(data => {
            localStorage.removeItem("cachedWorkout");
            loadActiveWorkoutExercises(data);
        })
        .catch(error => {
            console.log('Error occurred:', error);
        });
}



let dataMap = new Map();
// 
function handleSetDataLocalStorage() {
    console.log('Fired handleSetDataLocalStorage()');

    let parentElem = this.parentElement;

    let setId = parentElem.getAttribute("data-setid");
    let weightId = this.getAttribute("data-weight-setid")
    let repsId = this.getAttribute("data-reps-setid")

    // Grabs all IDs/classes from the selected (this context) table cell
    tdData = this.getAttributeNames();
    console.log(tdData);
    tdContent = this.textContent;

    // Stores the data in localstorage. The first index of the tdData array will always be the name of cell used to invoke this function. Concats the setId to serve as a unique key for localstorage
    localStorage.setItem(tdData[0]+"="+setId, tdContent);
}

function addSetButtonClicked() {
    let elementClicked = this;
    let parentElement = this.parentElement;

    // Grabs the exercise id and stores in a variable 
    let WorkoutExerciseId = parentElement.getAttribute("headerworkoutexerciseid")
    console.log(parentElement);

    // Deletes the cached workout so the new set can be reflected on the page for the user.
    localStorage.removeItem("cachedWorkout");
    fetch('/Workout/InsertSets?WorkoutExerciseId=' + WorkoutExerciseId)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            //queryActiveWorkoutExercises();
            //localStorage.removeItem("cachedWorkout");
            //console.log("Before querying...");
            //queryActiveWorkoutExercises();
        })
        .catch(error => {
            console.log('Error occurred:', error);
        });
}


function loadActiveWorkoutExercises(activeWorkoutObj) {
    let activeWorkoutContainer = document.querySelector("#activeWorkoutContainer");
    let generateTable = document.createElement("table");
    generateTable.setAttribute("class", "activeWorkoutTable");
    activeWorkoutContainer.appendChild(generateTable);

    // Dictates the list of columns to be displayed on the page
    const columnNames = ["SetsCount", "WeightPerSet", "RepsPerSet", "SetCategoryArray"];

    // Track the WorkoutExerciseIds we've already processed
    let processedWorkoutExerciseIds = new Set();

    // Iterate through each item in activeWorkoutObj
    activeWorkoutObj.forEach(workout => {
        //console.log(workout);

        // Only create a header row if one does not already currently exist for this WorkoutExerciseId (1 header only per exercise)
        if (!processedWorkoutExerciseIds.has(workout["WorkoutExerciseId"])) {
            // Mark this WorkoutExerciseId as processed
            processedWorkoutExerciseIds.add(workout["WorkoutExerciseId"]);

            // Create a new "header" row for each unique WorkoutExerciseId
            let newRowHead = generateTable.insertRow();
            newRowHead.className = "headerRow";
            newRowHead.setAttribute("headerworkoutexerciseid", workout["WorkoutExerciseId"]);
            newRowHead.addEventListener("click", collapseExerciseSets);

            let newCellHeadSetsCount = newRowHead.insertCell();
            let newCellHeadExerciseName = newRowHead.insertCell();

            let cellExerciseName = workout["ExerciseName"];
            let cellNoOfSetsValue = workout["SetsCount"];

            let textNodeExerciseName = document.createTextNode(cellExerciseName);
            let textNodeSets = document.createTextNode(cellNoOfSetsValue);

            newCellHeadSetsCount.appendChild(textNodeSets);
            newCellHeadExerciseName.appendChild(textNodeExerciseName);

            // Generate the button which marks a set as complete
            let addSetButton = document.createElement("button");
            addSetButton.setAttribute("class", "setButton");
            addSetButton.addEventListener("click", addSetButtonClicked);
            newRowHead.appendChild(addSetButton);
        }

        // Create a new row for each set of the workout
        let newRow = generateTable.insertRow();
        newRow.setAttribute("class", "exerciseRow");
        newRow.setAttribute("workoutexerciseid", workout["WorkoutExerciseId"]);
        newRow.setAttribute("data-setid", workout["SetId"])
        let weightPerSet = workout["Weight"];
        let repsPerSet = workout["Reps"];
        let setCategory = workout["SetCategory"];


        let weightCell = newRow.insertCell();
        let repsCell = newRow.insertCell();
        let categoryCell = newRow.insertCell();

        categoryCell.appendChild(document.createTextNode(setCategory));
        weightCell.appendChild(document.createTextNode(weightPerSet));
        // Used to uniquely identify the td for localstorage + database updates.
        weightCell.setAttribute("data-weight-setid", workout["SetId"])
        weightCell.setAttribute("contenteditable", "true");
        weightCell.addEventListener("keyup", handleSetDataLocalStorage);

        repsCell.appendChild(document.createTextNode(repsPerSet));
        // Used to uniquely identify the td for localstorage + database updates.
        repsCell.setAttribute("data-reps-setid", workout["SetId"])
        repsCell.setAttribute("contenteditable", "true");
        repsCell.addEventListener("keyup", handleSetDataLocalStorage)


        // Generate the button which marks a set as complete
        let setButton = document.createElement("button");
        setButton.setAttribute("class", "setButton");
        setButton.addEventListener("click", setButtonClicked);
        newRow.appendChild(setButton);


        // Generate the button which deletes a set
        let deleteSetButton = document.createElement("button");
        deleteSetButton.setAttribute("class", "deleteSetButton");
        deleteSetButton.addEventListener("click", deleteButtonClicked);
        deleteSetButton.appendChild(document.createTextNode("Del Set"));
        newRow.appendChild(deleteSetButton);


        
    });
    // Needs to run outside the above for loop or else it will run wayyy too many times. Loads from localstorage the sets marked as completed after elements have been generated
    loadLocalStorage();
}
function queryActiveWorkoutExercises() {
    fetch('/Workout/ViewActiveWorkoutByUserId?UserId=' + UserId)
        .then(response => response.json())
        .then(data => {
            // Caches the retrieved workout data into localstorage to prevent multiple API requests to the database upon page refresh
            localStorage.setItem("cachedWorkout", data);

            let parsedWorkoutObj = JSON.parse(data)
            // Loads from local storage the sets the user has marked as completed. This must be done here at the end of the promise chain, or else returns null elements in query selector.
            loadActiveWorkoutExercises(parsedWorkoutObj)
        })
        .catch(error => {
            console.log('Error occurred:', error);
        });

}


// Stores a GLOBAL list of exercise IDs that the user has clicked on to be added to their workout
let selectedExerciseIds = [];
function activeRows(tableData) {
    const tdId = tableData.getAttribute("data-exercise-id");
    //const tdId = td.id;
    const tdIndex = selectedExerciseIds.indexOf(tdId);

    if (tdIndex === -1) {
        selectedExerciseIds.push(tdId);
        tableData.style.background = "aqua";
    }
    else {
        selectedExerciseIds.splice(tdIndex, 1);
        tableData.style.background = "white";
    }
    console.log('Clicked rows: ', selectedExerciseIds);
};

function addExerciseBtnClicked() {

    fetch('/Workout/ViewExerciseList')
        .then(response => response.json())
        .then(data => {
            // Causes the modal to pop-up upon SQL query returning succesfully
            modal.style.display = "block";

            // Tells JS to expect and parse as a Json obj.
            var jsonArr = JSON.parse(data);

            let modalContent = document.querySelector(".dynamic-content");

            let generateTable = document.createElement("table");
            generateTable.setAttribute("id", "DynamicExerciseTable");
            modalContent.appendChild(generateTable);

            // Iterates through each exercise and displays it within its own table row attribute
            for (let i = 0; i < jsonArr.length; i++) {
                //console.log(jsonArr[i]);

                let exerciseName = jsonArr[i]["ExerciseName"];
                let exerciseId = jsonArr[i]["ExerciseId"]

                let newRow = generateTable.insertRow();

                let newCell = newRow.insertCell();
                newCell.setAttribute("data-exercise-id", exerciseId);

                let addContent = document.createTextNode(exerciseName);
                newCell.appendChild(addContent);

                // This cannot be a lambda function, as it requires the use of "this" (i.e. referencing itself) to work.
                newCell.addEventListener("click", function () {
                    newCell.classList.toggle("highlightCell")
                    
                    console.log(this.getAttribute("data-exercise-id"));
                    activeRows(this);
                    //this.style.background = "aqua";

                });
            }

            // Dynamically generates the submit exercise button at the bottom of the modal
            let submitExerciseBtn = document.createElement("button");
            submitExerciseBtn.setAttribute("id", "submitExerciseBtn");

            // Populates text inside the button
            submitExerciseBtn.appendChild(document.createTextNode("Submit exercises"));
            submitExerciseBtn.onclick = submitExercises;

            // Adds the button to the page as the last child element of the modal pop-up
            modalContent.after(submitExerciseBtn);


        })
        .catch(error => {
            console.error('Error fetching data: ', error);
        });


};

// The close button for the modal 
span.onclick = function () {
    modal.style.display = "none";

    // Grabs the parent node inside of the modal-content
    let modalContent = document.querySelector(".dynamic-content");
    let submitExerciseBtn = document.querySelector("#submitExerciseBtn");

    while (modalContent.firstChild) {
        modalContent.removeChild(modalContent.firstChild);
        submitExerciseBtn.remove();
    }

};

function collapseExerciseSets(event) {
    // The parent element of the row clicked
    let parentElement = this; 
    let workoutExerciseId = this.getAttribute("headerworkoutexerciseid");

    let matchingRows = document.querySelectorAll(`[workoutexerciseid='${workoutExerciseId}']`);
    console.log(matchingRows);

    // Toggle the display of each matching row
    // There is a bug that needs fixinghere. It takes two clicks to shrink a row header because it first has to apply display = table-row. 
    matchingRows.forEach(row => {
        if (row.style.display === "none" || row.style.display === "") {
            // sets the rows display back to a regular table row
            row.style.display = "table-row"; 
        } else {
            row.style.display = "none";
        };
    });
};






document.getElementById('start-button').addEventListener('click', startTimer);
document.getElementById('add-time-button').addEventListener('click', addTimeToTimer);

function startTimer() {
    let duration = 60 * 2; // Set the duration in seconds (e.g., 5 minutes)
    const countdownDisplay = document.getElementById('countdown-timer');
    const addTimeButton = document.querySelector("#add-time-button");



    let timerInterval = setInterval(() => {
        let minutes = Math.floor(duration / 60);
        let seconds = duration % 60;

        // Format minutes and seconds to be always two digits
        minutes = minutes < 10 ? '0' + minutes : minutes;
        seconds = seconds < 10 ? '0' + seconds : seconds;

        countdownDisplay.textContent = `${minutes}:${seconds}`;

        if (duration <= 0) {
            clearInterval(timerInterval);
            countdownDisplay.textContent = "Weight go brr!";
        }

        duration--;
    }, 1000);
};


function addTimeToTimer() {

}

let setCompleteArray = [];
function setButtonClicked(e) {
    let parentElem = this.parentElement;
    let setId = parentElem.getAttribute("data-setid");

    //console.log(setId);

    startTimer();


    if (parentElem.classList.contains("setComplete")) {
        parentElem.classList.remove("setComplete");
        // Remove the set ID from the global array based on the items index
        setCompleteArray.splice(setCompleteArray.indexOf(setId), 1);

        let stringifieSetIdsArray = JSON.stringify(setCompleteArray);
        localStorage.setItem("setComplete", stringifieSetIdsArray);
    } else {
        parentElem.classList.toggle("setComplete");
        setCompleteArray.push(setId);

        let stringifieSetIdsArray = JSON.stringify(setCompleteArray);
        localStorage.setItem("setComplete", stringifieSetIdsArray);
    }
    
}


function deleteButtonClicked() {
    console.log('delete buton clicked');
    const parentElem = this.parentElement;
    console.log(parentElem);
    // Grabs the set ID of the row that triggered the event listener (button press)
    let clickedSetId = parentElem.getAttribute("data-setid");
    console.log(clickedSetId);

    parentElem.remove();

    //// Deletes the cached workout so the new set can be reflected on the page for the user.
    //localStorage.removeItem("cachedWorkout");
    fetch('/Workout/RemoveSets?SetIds=' + clickedSetId)
        .then(response => response.json())
        .then(data => {
            console.log(data);
        })
        .catch(error => {
            console.log('Error occurred:', error);
        });

    // Retrieve the cachedWorkout array from localStorage
    let cachedWorkout = JSON.parse(localStorage.getItem('cachedWorkout')) || [];

    // Filter out the array object that matches the clicked setId AKA remove the row associated with the exercise
    cachedWorkout = cachedWorkout.filter(item => item.SetId != clickedSetId);

    // Save the updated array back to localStorage
    localStorage.setItem('cachedWorkout', JSON.stringify(cachedWorkout));

}


function submitButtonClicked() {
    //let setsCompleted = localStorage.getItem("setComplete");
    //console.log(setsCompleted);

    let retrieveStorage = localStorage.getItem("setComplete");
    if (retrieveStorage != null) {
        let readJson = JSON.parse(retrieveStorage);
        //console.log(readJson);
        let convertJsonToInt = readJson.map(item => Number(item));
        //console.log(convertJsonToInt);
        setCompleteArray = convertJsonToInt;
        //console.log('Final array: ' + setCompleteArray);


        /*var newObj = new Object();*/
        let setsData = [];

        // Iterates through each set marked as completed and unpacks their values i.e. set weight/reps associated with the specific SetId
        setCompleteArray.forEach(dataSetId => {

            let getRow = document.querySelector(`[data-setid='${dataSetId}']`);


            if (getRow != null) {
                

                setId = getRow.getAttribute("data-setid");
                //console.log("Set ID: " + setId);

                // Saves a HTML collection object of the children within each row marked as completed in setCompleteArray
                let childNodes = getRow.children;

                
                // Grabs the contents (number) of the weight & reps table cell within an exercise row
                const setWeight = childNodes[0].textContent;
                const setReps = childNodes[1].textContent;


                var SetData = {
                    setid: parseInt(setId),
                    weight: parseFloat(setWeight),
                    reps: parseInt(setReps)
                }

                //var newSetObj = {
                //    setid: setId,
                //    weight: childNodes[0].textContent,
                //    reps: childNodes[1].textContent
                //}

                setsData.push(SetData);




            }
        });
        console.log()
        console.log("Data being sent:", JSON.stringify({
            WorkoutData: setsData
        }));

        fetch('/Workout/SubmitWorkout?UserId=' + UserId, {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    WorkoutData: setsData
                })
            })
            .then(response => response.json())
            .then(data => {
                console.log(data);
            })
            .catch(error => {
                console.log('Error occurred:', error);
            });

    }

    const weightKey = Object.keys(localStorage).filter(key => key.startsWith('data-weight-setid'));
    const repKey = Object.keys(localStorage).filter(key => key.startsWith('data-reps-setid'));

    const weightKeyValuePairs = weightKey.map(weightKey => {
        return {
            key: weightKey,
            value: JSON.parse(localStorage.getItem(weightKey))
        };
    });

    const setKeyValuePairs = repKey.map(setKey => {
        return {
            key: setKey,
            value: JSON.parse(localStorage.getItem(setKey))
        };
    });
}


function loadLocalStorage() {
    // Checks if the set id tracking array is empty. If empty, skips writing the local storage data to it. Otherwise persistency between refreshes would not work.

    let retrieveStorage = localStorage.getItem("setComplete");
    if (retrieveStorage != null) {
        let readJson = JSON.parse(retrieveStorage);
        //console.log(readJson);
        let convertJsonToInt = readJson.map(item => Number(item));
        //console.log(convertJsonToInt);
        setCompleteArray = convertJsonToInt;
        //console.log('Final array: ' + setCompleteArray);


        setCompleteArray.forEach(dataSetId => {
            //console.log(`Searching for setid='${dataSetId}'`);

            let getRow = document.querySelector(`[data-setid='${dataSetId}']`);
            //console.log(getRow);
            if (getRow != null) {
                getRow.classList.toggle("setComplete");
            }
        });
    }
    
    const weightKey = Object.keys(localStorage).filter(key => key.startsWith('data-weight-setid'));
    const repKey = Object.keys(localStorage).filter(key => key.startsWith('data-reps-setid'));

    const weightKeyValuePairs = weightKey.map(weightKey => {
        return {
            key: weightKey,
            value: JSON.parse(localStorage.getItem(weightKey))
        };
    });

    const setKeyValuePairs = repKey.map(setKey => {
        return {
            key: setKey,
            value: JSON.parse(localStorage.getItem(setKey))
        };
    });

    // Unpacks the item from local storage based on key/value, splits the string to properly surround it in quote as a string, then finds the element based on its custom data attribute (id)
    weightKeyValuePairs.forEach(pair => {
        let key = pair.key;
        let value = pair.value;
            let [attributeName, attrValue] = key.split("=");
            let selector = `[${attributeName}="${attrValue}"]`;
            let getTd = document.querySelector(selector);

        if (getTd) {
            getTd.textContent = value;
        }
        else {
            console.log("Did not find an element: " + getTd);
        }

    })

    setKeyValuePairs.forEach(pair => {
        let key = pair.key;
        let value = pair.value;
        let [attributeName, attrValue] = key.split("=");
        let selector = `[${attributeName}="${attrValue}"]`;
        let getTd = document.querySelector(selector);

        if (getTd) {
            getTd.textContent = value;
        }
        else {
            console.log("Did not find an element: " + getTd);
        }

    })

}