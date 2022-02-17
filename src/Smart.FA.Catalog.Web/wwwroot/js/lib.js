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
