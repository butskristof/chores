import { format } from 'date-fns';

export const formatDate = (date) =>
  format(date instanceof Date ? date : new Date(date), 'dd/MM/yyyy');

export const formatDateAsJson = (date) => format(date, 'yyyy-MM-dd');
