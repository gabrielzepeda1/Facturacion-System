$(document).ready(function () { 
    console.log("DOM Ready...");
    $("input[type='checkbox']").checkboxradio();
    $("fieldset").controlgroup();
    responsive_grid();
    $("#btnNew").on("click", function () {

        open_popup()
    })
    $("#btnEditar").on("click", function () {

        open_popup()
    })





    function open_popup() {
        console.log("open_popup()..")
        $('#popup-form').bPopup({
            appendTo: 'form',
            speed: 650,
            transition: 'slideIn',
            transitionClose: 'slideBack',
            height: '200',
            width: '150',
        });
    }
    function close_popup() {
        console.log("close_popup()..")
        $('#popup-form').bPopup().close();
    }
    function responsive_grid() {
        
        //$('.datagird').basictable();
    }
})

