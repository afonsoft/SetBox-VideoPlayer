﻿!function(a){"function"==typeof define&&define.amd?define(["jquery","../jquery.validate"],a):"object"==typeof module&&module.exports?module.exports=a(require("jquery")):a(jQuery)}(function(a){return a.extend(a.validator.messages,{required:"Povinné zadať.",maxlength:a.validator.format("Maximálne {0} znakov."),minlength:a.validator.format("Minimálne {0} znakov."),rangelength:a.validator.format("Minimálne {0} a maximálne {1} znakov."),email:"E-mailová adresa musí byť platná.",url:"URL musí byť platná.",date:"Musí byť dátum.",number:"Musí byť číslo.",digits:"Môže obsahovať iba číslice.",equalTo:"Dve hodnoty sa musia rovnať.",range:a.validator.format("Musí byť medzi {0} a {1}."),max:a.validator.format("Nemôže byť viac ako {0}."),min:a.validator.format("Nemôže byť menej ako {0}."),creditcard:"Číslo platobnej karty musí byť platné.",step:a.validator.format("Musí byť násobkom čísla {0}.")}),a});