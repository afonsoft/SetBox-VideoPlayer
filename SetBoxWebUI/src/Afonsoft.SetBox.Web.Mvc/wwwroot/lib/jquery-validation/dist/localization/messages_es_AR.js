﻿!function(r){"function"==typeof define&&define.amd?define(["jquery","../jquery.validate"],r):"object"==typeof module&&module.exports?module.exports=r(require("jquery")):r(jQuery)}(function(r){return r.extend(r.validator.messages,{required:"Este campo es obligatorio.",remote:"Por favor, completá este campo.",email:"Por favor, escribí una dirección de correo válida.",url:"Por favor, escribí una URL válida.",date:"Por favor, escribí una fecha válida.",dateISO:"Por favor, escribí una fecha (ISO) válida.",number:"Por favor, escribí un número entero válido.",digits:"Por favor, escribí sólo dígitos.",creditcard:"Por favor, escribí un número de tarjeta válido.",equalTo:"Por favor, escribí el mismo valor de nuevo.",extension:"Por favor, escribí un valor con una extensión aceptada.",maxlength:r.validator.format("Por favor, no escribas más de {0} caracteres."),minlength:r.validator.format("Por favor, no escribas menos de {0} caracteres."),rangelength:r.validator.format("Por favor, escribí un valor entre {0} y {1} caracteres."),range:r.validator.format("Por favor, escribí un valor entre {0} y {1}."),max:r.validator.format("Por favor, escribí un valor menor o igual a {0}."),min:r.validator.format("Por favor, escribí un valor mayor o igual a {0}."),nifES:"Por favor, escribí un NIF válido.",nieES:"Por favor, escribí un NIE válido.",cifES:"Por favor, escribí un CIF válido."}),r});