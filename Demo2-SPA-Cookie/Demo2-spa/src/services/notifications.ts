import { Notify } from 'quasar'
import { ErrorObject } from '@vuelidate/core';

export function ErrorsToString(errors : ErrorObject[]) {
  if (errors !== undefined && errors.length > 0) {
    return errors[0].$message.toString();
  }
  return '';
}

export function showRequestError(status: number, title: string, detail : string, error : {[key : string] : string}) {
  Notify.create({
            message: `Status ${status}: ${title}`,
            type: 'negative',
            caption: detail + JSON.stringify(error),
            group: false,
            timeout: 0,
            closeBtn: true,
            position: 'top'
        });
}
