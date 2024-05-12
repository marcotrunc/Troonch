//# Constants

const errorCodes = {
    validation: 422,
    notFound: 400,
    internalServer: 500
}


//# Functions
const showErrors = (errors) => {
    const { validationErrors } = errors;
    const fields = validationErrors.map(e => e.field).filter((item, index, currentArray) => currentArray.indexOf(item) == index);

    fields.forEach(field => {
        let errorElement = document.querySelector(`[data-error="${field}"]`);
        errorElement.textContent = validationErrors.filter(error => error.field == field)[0].message;
    });
}

const cleanFormFromErrorMessage = () => {
    const errorElements = document.getElementsByClassName('error-message');

    for (let i = 0; i < errorElements.length; i++) {
        errorElements[i].textContent = ''
    }
};

const toogleValue = (element) => {
    const elements = document.getElementsByName(element.name);
    const value = elements[0].checked.toString();
    for (let i = 0; i < elements.length; i++) {
        elements[i].value = value;
    }
}

const disableForm = (id) => {
    const form = document.getElementById(id);
    [...form.elements].forEach(input => input.disabled = true);
}

const enableForm = (id) => {
    const form = document.getElementById(id);
    [...form.elements].forEach(input => input.disabled = false);
} 


const showNotification = (isInError, errorMessage = "Errore di validazione") => 
    Toastify({
        text: isInError ? errorMessage : "Operazione completata con successo",
        duration: 3000, 
        gravity: "top", 
        position: "right",
        backgroundColor: isInError ? "linear-gradient(to right, #FF512F,  #DD2476)": "linear-gradient(to right, #02AABD, #00CDAC)",
        stopOnFocus: true,
    }).showToast();

const handleFormInError = async (response, formId) => {
    const errorResult = await response.json();

    if (!errorResult) {
        throw new Error("ErrorResult not retrived");
    }

    if (response.status == errorCodes.validation) {
        showNotification(true);
        enableForm(formId);
        return showErrors(errorResult.error)
    }

    showNotification(true, errorResult.error.message);

    enableForm(formId);
}

const handleExceptionInFormWithRedirect = (error) => {
    console.error(error);
    window.location.href = `/Error/${errorCodes.notFound}`;
}

