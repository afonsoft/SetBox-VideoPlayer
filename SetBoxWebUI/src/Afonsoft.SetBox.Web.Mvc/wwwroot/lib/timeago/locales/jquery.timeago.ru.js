﻿!function(n){"function"==typeof define&&define.amd?define(["jquery"],n):"object"==typeof module&&"object"==typeof module.exports?n(require("jquery")):n(jQuery)}(function(n){function e(n,e,o,t){var u=(n%=100)%10;return 1===u&&(1===n||n>20)?e:u>1&&u<5&&(n>20||n<10)?o:t}n.timeago.settings.strings={prefixAgo:null,prefixFromNow:"через",suffixAgo:"назад",suffixFromNow:null,seconds:"меньше минуты",minute:"минуту",minutes:function(n){return e(n,"%d минуту","%d минуты","%d минут")},hour:"час",hours:function(n){return e(n,"%d час","%d часа","%d часов")},day:"день",days:function(n){return e(n,"%d день","%d дня","%d дней")},month:"месяц",months:function(n){return e(n,"%d месяц","%d месяца","%d месяцев")},year:"год",years:function(n){return e(n,"%d год","%d года","%d лет")}}});