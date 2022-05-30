alert("hello");
// Il existe moins de méthode pour sélectionner un nœud DOM
// avec les navigateurs historiques
var form = document.getElementsByTagName('form')[0];
var email = document.getElementById('formValidationEmailId');

// Ce qui suit est une bidouille pour atteindre le prochain nœud Element dans le DOM
// Attention à cette méthode, elle peut permettre de construire une boucle
// infinie. Pour les navigateurs plus récents, on utilisera element.nextElementSibling
var error = email;
while ((error = error.nextSibling).nodeType != 1);

// Pour respecter la spécification HTML5
var emailRegExp = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

// De nombreux navigateurs historiques ne supportent pas la méthode
// addEventListener. Voici une méthode simple (il en existe d'autres)
function addEvent(element, event, callback) {
    var previousEventCallBack = element["on" + event];
    element["on" + event] = function(e) {
        var output = callback(e);

        // Une fonction de rappel (callback) qui renvoie `false`
        // pour arrêter la chaîne des callback
        // et interrompre l'exécution du callback d'événement.
        if (output === false) return false;

        if (typeof previousEventCallBack === 'function') {
            output = previousEventCallBack(e);
            if (output === false) return false;
        }
    }
};

// On peut désormais reconstruire notre validation de contrainte
// Étant donné qu'on n'utilise pas la pseudo-classe CSS, il faut
// explicitement gérer la classe valid/invalid du champ e-mail
addEvent(window, "load", function() {
    // Ici, on teste si le champ est vide (rappel : le champ n'est pas obligatoire)
    // S'il ne l'est pas, on vérifie que son contenu est une adresse e-mail valide.
    var test = email.value.length === 0 || emailRegExp.test(email.value);

    email.className = test ? "valid" : "invalid";
});

// Ici, on définit ce qui se passe lorsque l'utilisateur
// saisit quelque chose dans le champ
addEvent(email, "keyup", function() {
    var test = email.value.length === 0 || emailRegExp.test(email.value);
    if (test) {
        email.className = "valid";
        error.innerHTML = "";
        error.className = "error";
    } else {
        email.className = "invalid";
    }
});

// Ici, on définit ce qui se passe lorsque l'utilisateur
// tente d'envoyer les données du formulaire
addEvent(form, "submit", function() {
    //alert("hello");
    var test = email.value.length === 0 || emailRegExp.test(email.value);

    if (!test) {
        email.className = "invalid";
        error.innerHTML = "Merci d'écrire une adresse e-mail valide.";
        error.className = "error active";

        // Certains navigateurs historiques ne supportent pas
        // la méthode event.reventDefault()
        return false;
    } else {
        email.className = "valid";
        error.innerHTML = "";
        error.className = "error";
    }
});
