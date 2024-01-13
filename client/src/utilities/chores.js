import { addDays, differenceInCalendarDays } from 'date-fns';

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
  if (last == null) return null;
  const expectedNext = addDays(last, chore.interval);
  return differenceInCalendarDays(expectedNext, new Date());
};
