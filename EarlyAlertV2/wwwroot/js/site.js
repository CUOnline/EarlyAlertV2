// Write your JavaScript code.

function addRowHandlers(tableId, createClickHandler) {
    var table = document.getElementById(tableId);
    var rows = table.getElementsByTagName("tr");
    for (i = 0; i < rows.length; i++) {
        var currentRow = table.rows[i];
        currentRow.onclick = createClickHandler(currentRow);
    }
}