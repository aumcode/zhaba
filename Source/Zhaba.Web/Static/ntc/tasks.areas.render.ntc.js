/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Areas.Render = (function () {
    "use strict";
    var published = {};

    published.buildGrid = function (root, areas) {
        /***
         div 
         {
            class=rst-table
            div=Counter     { class="rst-cell rst-details-head" style="width:10%" }
            div=Name        { class="rst-cell rst-details-head" style="width:80%"}
            div=Linked      { class="rst-cell rst-details-head" style="width:10%"}
            "?for(var i=0, l=areas.length; i<l; i++)" {
                div="?areas[i][2]" { class="rst-cell rst-details-cell" style="width:10%"}
                div="?areas[i][3]" { class="rst-cell rst-details-cell" style="width:80%"}
                div 
                {
                    class="rst-cell rst-details-cell rst-text-align-center"
                    style="width:10%"
                    input="?areas[i][4]"
                    {
                        style="display:inline-block"
                        type='checkbox'
                        checked="?areas[i][4]"
                        onchange="?'linkIssueArea(event,\"{0}\", \"{1}\", \"{2}\")'.args(areas[i][0],areas[i][1],areas[i][2])"
                    }
                }
            }
         }
         
         ***/
    };

    return published;
})();