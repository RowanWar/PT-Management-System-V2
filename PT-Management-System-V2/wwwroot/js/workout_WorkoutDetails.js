document.addEventListener("DOMContentLoaded", function () {
    // Get the table and tbody
    let table = document.querySelector(".table");
    let tbody = table.getElementsByTagName("tbody")[0];
    let rows = Array.from(tbody.getElementsByTagName("tr"));

    let columnIndex = 2; // Change this to the column index you want to filter by (0-based index)

    // Function to extract unique values from the specified column
    function getUniqueValues() {
        let uniqueValues = new Set();
        rows.forEach(function (row) {
            let cells = row.getElementsByTagName("td");
            let cellValue = cells[columnIndex].textContent || cells[columnIndex].innerText;
            uniqueValues.add(cellValue.trim());
        });
        return Array.from(uniqueValues);
    }

    // Extract unique values from the specified column
    let uniqueValues = getUniqueValues();

    // Iterate through the unique values to filter and toggle the table
    uniqueValues.forEach(function (filterValue) {
        filterTable(filterValue);
        console.log(filterValue);
    });

    function filterTable(filterValue) {
        let seenValues = new Set();

        rows.forEach(function (row, index) {
            let cells = row.getElementsByTagName("td");
            let cellValue = cells[columnIndex].textContent || cells[columnIndex].innerText;

            if (cellValue.indexOf(filterValue) > -1) {
                if (!seenValues.has(cellValue)) {
                    seenValues.add(cellValue);
                    row.style.display = "";
                    addToggle(row, cellValue);
                }
                else {
                    // Hides any rows after the first "parent row" i.e shows exerciseGroupId set 0 but not set 1, 2, etc.
                    row.style.display = "none";
                    row.classList.add("child-hidden-row");
                }
            }
            else if (seenValues.has(cellValue)) {
                // If the row's cell value does not match any seen value, it should be visible
                row.style.display = "none";
                row.classList.add("child-hidden-row");
            }
        });
    }

    function addToggle(row, cellValue) {
        let toggleBtn = document.createElement('span');
        toggleBtn.textContent = "Toggle";
        toggleBtn.className = "toggle-btn";
        toggleBtn.style.marginLeft = "10px";
        toggleBtn.style.cursor = "pointer";
        toggleBtn.style.color = "blue";
        toggleBtn.style.textDecoration = "underline";
        toggleBtn.addEventListener('click', function () {
            rows.forEach(function (r) {
                let cells = r.getElementsByTagName("td");
                let value = cells[columnIndex].textContent || cells[columnIndex].innerText;

                // Handles the toggle button hiding rows
                if (value === cellValue && r !== row) {
                    if (r.style.display === "none") {
                        r.style.display = "";
                    } else {
                        r.style.display = "none";
                    }
                }
            });
        });

        let cell = row.getElementsByTagName("td")[columnIndex];
        cell.appendChild(toggleBtn);
    }
});