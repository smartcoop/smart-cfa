$(document).ready(
    function(event)
    {
        let validateButton = document.getElementById("Save");
        validateButton.addEventListener("click", function(e)
        {
            let model = document.getElementById("isDraft");
            model.value = true;
        });
    });

// Scrolls to the first HTML element that has a class attribute ending with '--error'
function scrollToFirstErrorElement() {
    let elementsInError = document.querySelectorAll("*[class*='--error']");
    if (elementsInError.length) {
        let firstInvalidControl = elementsInError[0];
        let labelOffset = 500;
        window.scroll({
            top: (firstInvalidControl.getBoundingClientRect().top + window.scrollY - labelOffset),
            left: 0,
            behavior: "smooth"
        });
        firstInvalidControl.focus();
    };
}

function RegisterScrollToFirstElementInError() {
    window.addEventListener("load", scrollToFirstErrorElement);
}
