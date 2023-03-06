import formatter from 'date-format'

export function formatDate(date : Date | string | null | undefined, format? : string) {
  if (date === undefined || date === null) return '';
  else {
    format ??= 'dd.MM.yyyy. hh:mm:ss'
    if (typeof date === 'string') {
      date = new Date(date)
      return Number.isNaN(date.valueOf()) ? '' : formatter(format, date);
    }
    else
      formatter(format, date);
  }
}
