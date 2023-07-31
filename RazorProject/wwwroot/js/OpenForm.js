// Get references to the modal container and the form open/close buttons
const modalContainer = document.getElementById('modalContainer');
const openFormButton = document.getElementById('openFormButton');
const closeFormButton = document.getElementById('closeFormButton');


// Function to open the form
function openForm() {
    
    modalContainer.style.display = 'block';
}

// Function to close the form
function closeForm() {
    modalContainer.style.display = 'none';
}

// Event listeners for the form open/close buttons
openFormButton.addEventListener('click', openForm);
closeFormButton.addEventListener('click', closeForm);


// Optionally, you can prevent the form from being submitted (avoiding page reload) by adding an event listener to the form submission event:
document.getElementById('myForm').addEventListener('submit', function (e) {
    e.preventDefault();
    // Process form data here and handle form submission
    // Optionally, close the form after submission
    closeForm();
});
