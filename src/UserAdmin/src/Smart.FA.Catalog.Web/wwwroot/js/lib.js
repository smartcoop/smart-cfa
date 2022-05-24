$(document).ready(
    // This is used only by the creation and edition page therefore it could be moved somewhere else.
    // If one has the heart to do so he/she is welcome :).
    // Btw try avoiding jQuery please :(.
    function(event)
    {
        let validateButton = document.getElementById("Save");
        if (!validateButton) {
           return;
        }
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

        // The way to scroll to a div element is a bit different than inputs.
        if (firstInvalidControl.tagName === "div" || firstInvalidControl.tagName === "DIV") {
            firstInvalidControl.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });
            return;
        }
        window.scroll({
            top: (firstInvalidControl.getBoundingClientRect().top + window.scrollY - labelOffset),
            left: 0,
            behavior: "smooth"
        });
        firstInvalidControl.focus();
    }
}

function RegisterScrollToFirstElementInError() {
    window.addEventListener("load", scrollToFirstErrorElement);
}

// Handling of the culture switching.
document.addEventListener("DOMContentLoaded",
    function() {
        RegisterCultureChange();
    });

function ChangeCulture(culture) {
    // this is the cookie asp expects to have. Note that the path is set to '/'.
    let cookie = `.AspNetCore.Culture=c=${culture}|uic=${culture};path=/`; //"=en|uic=en"
    document.cookie = cookie;

    // We can reload the page to the user is happy.
    location.reload();
}

function RegisterCultureChange() {
    document.getElementById("culture").addEventListener("change",
        function(event) {
            ChangeCulture(event.target.value);
        });
}

function disableEnterKeyUpKeyPressOnForm() {
    var forms = document.getElementsByTagName('form');
    for (var i = 0; i < forms.length; i++) {
        forms[i].addEventListener('keydown', ignoreEnterKeyEventHandler, true);
        forms[i].addEventListener('keyup', ignoreEnterKeyEventHandler, true);
    }

    function disableEnterKeyEvent(e) {
        if (e.keyIdentifier === 'U+000A' || e.keyIdentifier === 'Enter' || e.keyCode === 13) {
            e.preventDefault();
            return false;
        }
        return true;
    }
}

// function ignoreEnterKeyEventHandler(e) {
//     if (e.keyIdentifier === 'U+000A' || e.keyIdentifier === 'Enter' || e.keyCode === 13) {
//         e.preventDefault();
//         return false;
//     }
//     return true;
// }
