/// <reference path="jquery-1.11.1.js" />
$(document).ready(function () {
    $("#btnI").click(function () {
        var paginaEntera = document.body.innerHTML;

        $(".ocultar").css("display", "none");
        $(".titulo").css("display", "block");

        var imprimirZona = document.getElementById("imprimirTabla").innerHTML;

        document.body.innerHTML = "<html><head><title></title></head><body>" + imprimirZona + "</body>";
        window.print();

        $(".ocultar").css("display", "block");
        $(".titulo").css("display", "none");

        document.body.innerHTML = paginaEntera;
    });

    $("#btnImprimir").click(function () {
        var paginaEntera = document.body.innerHTML;
        var imprimirZona = document.getElementById("ordenEntera").innerHTML;
        document.body.innerHTML = "<html><head><title></title></head><body>" + imprimirZona + "</body>";
        window.print();
        document.body.innerHTML = paginaEntera;
    });

    $("#verTrabajos").click(function () {
        $("#btnImprimir").css("display", "inline");
        $("#btnOcultar").css("display", "inline");
        $("#lblTrabajo").css("display", "none");
        $("#tablaTrabajos").css("display", "inline");
        $("#verTrabajos").css("display", "none");
        $("#btnVolver").css("display", "inline");
    });

    $("#btnOcultar").click(function () {
        $("#btnImprimir").css("display", "none");
        $("#btnOcultar").css("display", "none");
        $("#lblTrabajo").css("display", "inline");
        $("#tablaTrabajos").css("display", "none");
        $("#verTrabajos").css("display", "block");
        $("#btnVolver").css("display", "none");
    });

    $("#btnOrden").click(function () {
        $("#fldGrabar").css("display", "none");
        $("#botones").css("display", "none");
        $("#fldFinal").css("display", "block");
    });

    $("#btnCancelar").click(function () {
        $("#fldGrabar").css("display", "block");
        $("#botones").css("display", "block");
        $("#fldFinal").css("display", "none");
    });

    $("#btnRegistro").click(function () {
        $("#frmBusqueda").css("display", "none");
        $("#frmRegistro").css("display", "block");
    });

    $("#btnCancelar").click(function () {
        $("#frmBusqueda").css("display", "block");
        $("#frmRegistro").css("display", "none");
    });

    $(".datepicker").datepicker();

    //SETEAR LOS DATEPICKER PARA CAMBIAR FORMATO DE FECHA Y SETEAR EL FIN DE LA FECHA   
    var fechaHoy = new Date();
    $("#fechaFin").datepicker("setDate", fechaHoy);

    $(".datepicker").datepicker("option", "dateFormat", "dd/mm/yy");
    $(".datepicker").datepicker("option", "dayNames", ["Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado"]);
    $(".datepicker").datepicker("option", "dayNamesMin", ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"]);
    $(".datepicker").datepicker("option", "monthNames", ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"]);
    $(".datepicker").datepicker("option", "showAnim", "slideDown");
    $(".datepicker").datepicker("option", "maxDate", "0");

    $(".numerico").keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });

    $(".letras").keypress(function (event) {
        var tecla = event.which;
        if ((tecla > 47 && tecla < 58) && (tecla != 32)) {
            return false;
        }
    });

    $(".nada").keypress(function (e) {
        var tecla = event.which;
        if (tecla > 1) {
            return false;
        }
    });

    function aparecer(x) {
        formFiltro = document.getElementById("frmFiltro");
        formRegistro = document.getElementById("frmRegistro");

        if (x == 1) {
            formFiltro.style.display = 'block';
            formRegistro.style.display = 'none';
        }
        else
            if (x == 2) {
                formFiltro.style.display = 'none';
                formRegistro.style.display = 'block';
            }
    };
});

    
    