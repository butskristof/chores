import { addDays, differenceInCalendarDays } from 'date-fns';

export const CHORE_DUE_STATES = {
  OK: 'OK',
  ALMOST_DUE: 'ALMOST_DUE',
  OVERDUE: 'OVERDUE',
};

export const getChoreDueState = (chore) => {
  const dueDays = getDueDays(chore);
  if (dueDays < 0) return CHORE_DUE_STATES.OVERDUE;
  else if (dueDays <= 1) return CHORE_DUE_STATES.ALMOST_DUE;
  return CHORE_DUE_STATES.OK;
};

export const getLastIteration = (chore) => {
  const mostRecent =
    chore.lastIteration ??
    chore.iterations?.reduce(
      (max, current) =>
        // eslint-disable-next-line no-nested-ternary
        max == null ? current : new Date(current.date) > new Date(max.date) ? current : max,
      null,
    )?.date;
  return mostRecent == null ? null : new Date(mostRecent);
};

export const getDueDays = (chore, lastIteration = null) => {
  const last = lastIteration ?? getLastIteration(chore);
  if (last == null) return 0;
  const expectedNext = addDays(last, chore.interval);
  return differenceInCalendarDays(expectedNext, new Date());
};
