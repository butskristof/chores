import { addDays, differenceInCalendarDays } from 'date-fns';

export const CHORE_DUE_STATES = {
  OK: 'OK',
  ALMOST_DUE: 'ALMOST_DUE',
  OVERDUE: 'OVERDUE',
};

export const addPropsToChore = (chore) => {
  if (chore == null) return null;
  const copy = { ...chore };
  if (copy.lastIteration == null) {
    const iteration = copy.iterations?.reduce(
      (max, current) =>
        // eslint-disable-next-line no-nested-ternary
        max == null ? current : new Date(current.date) > new Date(max.date) ? current : max,
      null,
    );
    copy.lastIteration = iteration?.date;
  }

  // due days
  if (copy.lastIteration == null) copy.dueDays = 0;
  else {
    const expectedNext = addDays(copy.lastIteration, copy.interval);
    copy.dueDays = differenceInCalendarDays(expectedNext, new Date());
  }

  // state
  if (copy.dueDays < 0) copy.dueState = CHORE_DUE_STATES.OVERDUE;
  else if (copy.dueDays <= 1) copy.dueState = CHORE_DUE_STATES.ALMOST_DUE;
  else copy.dueState = CHORE_DUE_STATES.OK;

  return copy;
};

export const getDueDaysMessage = (chore) => {
  const { dueDays } = chore;
  if (dueDays < 0) return `due ${Math.abs(dueDays)} days ago`;
  else if (dueDays === 1) return `due tomorrow`;
  else if (dueDays === 0) return `due today`;
  return `due in ${dueDays} days`;
};

const CHORE_DUE_STATE_ICONS = {
  [CHORE_DUE_STATES.OK]: 'pi pi-check',
  [CHORE_DUE_STATES.ALMOST_DUE]: 'pi pi-exclamation-triangle',
  [CHORE_DUE_STATES.OVERDUE]: 'pi pi-times',
};

export const getDueStateIcon = (chore) => CHORE_DUE_STATE_ICONS[chore.dueState];
