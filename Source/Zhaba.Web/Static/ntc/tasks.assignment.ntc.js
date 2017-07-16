/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Assignment = (function () {
    var published = {}
    ;
    
    published.buildAssignmentTab = function(root, task) {
        var gridID = "assignment-grid-" + task.Counter;
        ZHB.Tasks.Assignment.Render.buildAssignmentButtons(root, task);
        ZHB.Tasks.Render.createGrid(root, gridID);
        ZHB.Tasks.Assignment.Render.createAssignmentHeader(gridID);
        for (var j = 0, l = task.Assignments.length; j < l; j++)
            ZHB.Tasks.Assignment.Render.createAssignmentGridRow(gridID, task.Assignments[j], task);
    };
    
    published.init = function (init) {
        
    };
    
    return published;
})();