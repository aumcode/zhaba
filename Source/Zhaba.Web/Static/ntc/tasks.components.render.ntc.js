/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Components.Render = (function () {
    "use strict";
    var published = {};

    published.buildGrid = function (root, components) {
        /***
         div 
         {
            class=rst-table
            div=Counter     { class="rst-cell rst-details-head" style="width:10%" }
            div=Name        { class="rst-cell rst-details-head" style="width:80%"}
            div=Linked      { class="rst-cell rst-details-head" style="width:10%"}
            "?for(var i=0, l=components.length; i<l; i++)" {
                div="?components[i][2]" { class="rst-cell rst-details-cell" style="width:10%"}
                div="?components[i][3]" { class="rst-cell rst-details-cell" style="width:80%"}
                div 
                {
                    class="rst-cell rst-details-cell rst-text-align-center"
                    style="width:10%"
                    input="?components[i][4]"
                    {
                        style="display:inline-block"
                        type='checkbox'
                        checked="?components[i][4]"
                        onchange="?'linkIssueComponent(event,\"{0}\", \"{1}\", \"{2}\")'.args(components[i][0],components[i][1],components[i][2])"
                    }
                }
            }
         }
         
         ***/
    };

    return published;
})();