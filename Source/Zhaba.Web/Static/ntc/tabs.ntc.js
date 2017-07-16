/*function getStatusBarStyle(value) {
  var red = 0;
  var green = 0;
  if (value <= 25)
    red = 255;
  else if (value > 25 && value < 65) {
    red = 255;
    green = (value - 25) * 6;
  } else if (value == 65) {
    red = 255;
    green = 255;
  } else if (value > 65) {
    red = 255 - ((value - 65) * 7);
    green = 255;
  } else if (value == 100) {
    red = 0;
    green = 255;
  }
  return "width: {0}%; background: rgb({1},{2}, 0)".args(value, red, green);
}

function getStatusStyle(value) {
  return "status-tag {0}".args(value.toLowerCase());
}

function getPriorityStyle(value) {
  var priority;
  if (value === 0) {
    priority = "highest";
  } else if (value > 0 && value <= 3) {
    priority = "high";
  } else if (value > 3 && value <= 5) {
    priority = "middle";
  } else {
    priority = "lower";
  }
  return "priority-tag {0}".args(priority);
}

function buildDate(task) {
  var startDate = WAVE.dateTimeToString(task.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  var completeDate = "OPEN";
  if (task.Complete_Date) {
    completeDate = WAVE.dateTimeToString(task.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  }
  return "{0} - {1}".args(startDate, completeDate);
}

function buildDueDate(task) {
  var dueDate = WAVE.dateTimeToString(task.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  return "{0} in {1}d".args(dueDate, task.Remaining);
}*/




