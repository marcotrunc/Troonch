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
        try {
            let errorElement = document.querySelector(`[data-error="${field}"]`);
            errorElement.textContent = validationErrors.filter(error => error.field == field)[0].message;
        } catch (error) {
            console.error(error);
        }
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

const setPayloadFromFormData = (formData) => {
    let payload = {};
    formData.forEach((value, key) => {
        if (value == 'false') value = false;
        if (value == 'true') value = true;
        payload[key] = value;
    });

    return payload;
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

const renderHTML = async (action, containerId, modalId = null) => {
    try {
        
        const response = await fetch(action);


        if (!response.ok) {
            return await handleRequestInError(response);
        }


        const htmlFormContent = await response.text();

        if (htmlFormContent) {
            const modalBody = document.getElementById(containerId);
            modalBody.innerHTML = htmlFormContent;
        }

        if (modalId != null) {
            const variationModal = new bootstrap.Modal(document.getElementById(modalId));
            variationModal.show();
        }

    } catch (error) {
        handleExceptionInFormWithRedirect(error);
    }
}


const handleRequestInError = async (response, formId = null) => {
    const errorResult = await response.json();

    if (!errorResult) {
        throw new Error("ErrorResult not retrived");
    }

    if (formId != null && response.status == errorCodes.validation) {
        showNotification(true);
        enableForm(formId);
        return showErrors(errorResult.error)
    }

    showNotification(true, errorResult.error.message);

    if (formId != null) {
        enableForm(formId);
    }
}

const handleExceptionInFormWithRedirect = (error) => {
    console.error(error);
    window.location.href = `/Error/${errorCodes.internalServer}`;
}

const handleInput = (element) => {
    debugger;

    const inputElement = document.getElementById('original-price');
    console.log(inputElement);

    inputElement.addEventListener('input', function (event) {
        // Prevent the default behavior
        event.preventDefault();

        // Get the value typed by the user
        var typedValue = event.target.value;

        // Replace any commas with periods
        var modifiedValue = typedValue.replace(',', '.');

        // Set the modified value back to the input
        event.target.value = modifiedValue;

        // Now you can use 'modifiedValue' as needed
        console.log('Typed value:', typedValue);
        console.log('Modified value:', modifiedValue);
    });
}
