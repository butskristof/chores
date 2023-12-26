import { addDays, differenceInCalendarDays } from 'date-fns';

export const getLastIteration = (chore) => {
  const value =
    chore.lastIteration ??
    chore.iterations.reduce(
      (max, current) =>
        // eslint-disable-next-line no-nested-ternary
        max == null ? current : new Date(current.date) > new Date(max.date) ? current : max,
      null,
    )?.date;
  return value == null ? null : new Date(value);
};

export const getDueDays = (chore, lastIteration = null) => {
  const last = lastIteration ?? getLastIteration(chore);
  if (last == null) return null;
  const expectedNext = addDays(last, chore.interval);
  return differenceInCalendarDays(expectedNext, new Date());
};
