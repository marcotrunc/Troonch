﻿//# Constants

const errorCodes = {
    validation: 422,
    notFound: 400,
    internalServer: 500
}

const guidEmpty = '00000000-0000-0000-0000-000000000000';


//# Functions

const getControllerNameFromLocation = () => {
    const url = window.location.href;
    const parser = document.createElement('a');
    parser.href = url;

    const pathSegments = parser.pathname.split('/');
    let controllerName = null;
    
    if (pathSegments.length > 1) {
        controllerName = pathSegments[1];
    }

    return controllerName;
}


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

const enableForm = (id, fieldIdToNotDisable = []) => {
    const form = document.getElementById(id);
    console.log([...form.elements].filter(element => !fieldIdToNotDisable.includes(element.id)));
    [...form.elements]
        .filter(element => !fieldIdToNotDisable.includes(element.id))
        .forEach(input => input.disabled = false);
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

const decodeHtmlEntities = (htmlString) => {
    
    var tempElement = document.createElement('div');
    tempElement.innerHTML = htmlString;

    var decodedString = tempElement.textContent || tempElement.innerText;
    return decodedString;
}

const showNotification = (isInError, message) => {

    if (isInError && !message) {
        message = "Operation not completed!"
    }

    if (!isInError && !message) {
        message = "Operation completed successfully"
    }

    
    return Toastify({
                text: decodeHtmlEntities(message),
                duration: 3000, 
                gravity: "top", 
                position: "right",
                backgroundColor: isInError ? "linear-gradient(to right, #FF512F,  #DD2476)": "linear-gradient(to right, #02AABD, #00CDAC)",
                stopOnFocus: true,
            }).showToast();
}


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


const handleRequestInError = async (response, formId = null, fieldIdToNotDisable = []) => {
    const errorResult = await response.json();

    if (!errorResult) {
        throw new Error("ErrorResult not retrived");
    }

    if (formId != null && response.status == errorCodes.validation) {
        showNotification(true);
        enableForm(formId, fieldIdToNotDisable);
        return showErrors(errorResult.error)
    }

    if (response.status == errorCodes.internalServer) {
        handleExceptionInFormWithRedirect(errorResult.error.message);
    } 

    showNotification(true, errorResult.error.message);

    if (formId != null) {
        enableForm(formId, fieldIdToNotDisable);
    }
    
}

const handleExceptionInFormWithRedirect = (errorMessage = "") => {
    window.location.href = `/Error/${errorCodes.internalServer}/${errorMessage}`;
}

const closeModal = (modalId) => {
    const modal = document.getElementById(modalId);
    const modalInstance = bootstrap.Modal.getInstance(modal)
    modalInstance.hide();
}

const resetFormById = (formId) => {
    document.getElementById(formId).reset();
}

const handleNotificationFromServer = (boolValue, message) => {

    try {
        var isOperationSucceeded = new Boolean(boolValue);
        showNotification(!isOperationSucceeded, message);
    }
    catch (error) {
        console.error(error);
    }
}


const showConfiramtionModal = async(title, answer, functionParam) => {

    let titleElement = document.getElementById('confirm-modal-title');
    if (!titleElement || !title) {
        return;
    }
    titleElement.innerHTML = title;

    let answerElement = document.getElementById('confirm-modal-answer');
    if (!answerElement || !answer) {
        return;
    }
    answerElement.innerHTML = answer;


    let confirmButton = document.getElementById('confirm-modal-button');
    if (!confirmButton || !functionParam) {
        return;
    }
    confirmButton.addEventListener('click', functionParam);


    const confirmUserModal = new bootstrap.Modal(document.getElementById('confirm-modal'));
    confirmUserModal.show();

}