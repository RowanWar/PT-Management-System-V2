let parentDiv = document.querySelector('.profileContainerDiv');
let grabEditableDivs = document.querySelectorAll('.editable')
function buttonClicked() {
    console.log('This worked');
    console.log(grabEditableDivs);

    grabEditableDivs.forEach((element) => console.log(element));
    //parentDiv.editable.add(".Editing")

}
