﻿!function(n){"function"==typeof define&&define.amd?define(["jquery"],n):"object"==typeof module&&"object"==typeof module.exports?n(require("jquery")):n(jQuery)}(function(n){function e(n,e,i){var t=n%10;return t>1&&t<5&&(n>20||n<10)?e:i}n.timeago.settings.strings={prefixAgo:null,prefixFromNow:"za",suffixAgo:"temu",suffixFromNow:null,seconds:"mniej niż minutę",minute:"minutę",minutes:function(n){return e(n,"%d minuty","%d minut")},hour:"godzinę",hours:function(n){return e(n,"%d godziny","%d godzin")},day:"dzień",days:"%d dni",month:"miesiąc",months:function(n){return e(n,"%d miesiące","%d miesięcy")},year:"rok",years:function(n){return e(n,"%d lata","%d lat")}}});