//document.addEventListener("DOMContentLoaded", function () {
//    let addExerciseBtn = document.querySelector("#addExerciseBtn")
//    let formDataExerciseName = document.querySelector("#exerciseName")
//    let formDataExerciseDesc = document.querySelector("#exerciseDesc")
//});

const exerciseForm = document.querySelector("#exerciseForm")
let formDataExerciseName = document.querySelector("#exerciseName")
let formDataExerciseDesc = document.querySelector("#exerciseDesc")

exerciseForm.addEventListener("submit", (e) => {
    e.preventDefault();
    console.log('Button clicked');
    localStorage.clear();

    localStorage.setItem(formDataExerciseName, formDataExerciseName.value);
    localStorage.setItem(formDataExerciseDesc, formDataExerciseDesc.value);

    let result = localStorage.getItem(formDataExerciseName);
    let result2 = localStorage.getItem(formDataExerciseDesc);
    console.log('Test');
    console.log(result);
    console.log(result2);
})