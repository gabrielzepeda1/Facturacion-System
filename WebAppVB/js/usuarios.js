

//function tableActive() {
//    let table = document.getElementById("CP1_GridViewOne")


//    table.addEventListener("click", function (event) {
//        console.log("Table clicked")
//        console.log(event.target.parentElement)
//        console.log(event.target.parentElement.parentElement)

//        let tr = event.target.parentElement;
//        let tbody = event.target.parentElement.parentElement;
//        let rows = tbody.getElementsByTagName("tr");

//        console.log(rows)

//        if (event.target.parentElement.tagName === "TR") {
//            console.log("clicked tr")

//            for (let i = 0; i <= rows.legth; i++) {
//                rows[i].classList.remove("table-success");
//            }

//            tr.classList.add("table-success");
//            console.log("completed table success")
//        }

//    })
//}