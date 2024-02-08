// Este procedimiento formatea agregado comas cada tres digitos y acotando el numero de decimales segun las variables enviadas
//
// control     : Control enviado desde el codigo HTML. Al llamar el evento se coloca "this"
// num_decimal : Cantidad de decimales que mostrara el numero al ser formateado
//
function formatNumber(control, num_decimal) {
    var nums = new Array();
    var simb = ","; //Éste es el separador
    valor = control.value;

    var numeroLetras = valor.length; // numero de caracteres

    if (control.value.indexOf('.') > 0) {

        var posicion_decimal = valor.indexOf('.'); // Posicion del punto en la cadena
        var decimal = valor.substring(posicion_decimal); // Se obtiene el valor numerico decimal sin el punto

        decimal = decimal.replace(".", ""); // Se eliminan todos los puntos
        decimal = decimal.replace(/\D/g, "");   //Ésta expresión regular solo permitira ingresar números
        decimal = decimal.substring(0, num_decimal) // Se acota el numero para que se muesten solo la cantidad de decimales especificada
        valor = valor.substring(0, posicion_decimal) // Se elimina el valor decimla del valor numerico

    }

    valor = valor.replace(/\D/g, "");   //Ésta expresión regular solo permitira ingresar números

    nums = valor.split(""); //Se vacia el valor en un arreglo
    var long = nums.length - 1; // Se saca la longitud del arreglo
    var patron = 3; //Indica cada cuanto se ponen las comas
    var prox = 2; // Indica en que lugar se debe insertar la siguiente coma
    var res = "";

    while (long > prox) {
        nums.splice((long - prox), 0, simb); //Se agrega la coma
        prox += patron; //Se incrementa la posición próxima para colocar la coma

    }

    for (var i = 0; i <= nums.length - 1; i++) {
        res += nums[i]; //Se crea la nueva cadena para devolver el valor formateado
    }


    if (control.value.indexOf('.') > 0) {
        var enviar = res + "." + decimal
    }
    else {
        var enviar = res
    }

    control.value = enviar;
}

// Esta funcion verifica si el textbox esta lleno al disparar el evento focus y limpia el contenido.
// control     : Control enviado desde el codigo HTML. Al llamar el evento se coloca "this"
//
function control_clear(control) {
    var valor = control.value;

    if (valor == "0" || valor == "0.0" || valor == "0.00" || valor == "0.000" || valor == "0.0000") {
        control.value = "";
    }
}

// Esta funcion verifica si el textbox esta lleno al disparar el evento focus y limpia el contenido.
// control     : Control enviado desde el codigo HTML. Al llamar el evento se coloca "this"
//
function control_fill(control) {
    var valor = control.value;

    if (valor == "") {
        control.value = "0.00";
    }
}