export type RequestErrorCallback = (status : number, title:string, detail : string, error: {[key: string] : string}) => void;

export async function fetchItems<T>(endpoint : string, errorCallback? :  RequestErrorCallback) : Promise<T[]>
{
    const response = await fetch(`api/${endpoint}`);
    //const response = await fetch(`http://localhost:5029/${endpoint}`, { credentials: 'include' });
    if (response.ok) {
        const data = await response.json() as T[];
        return data;
    }
    else {
      await processError(response, errorCallback)
      return []
    }
}

export async function login(username : string, password : string, errorCallback? : RequestErrorCallback) : Promise<boolean>
{
    const url = 'api/account/login'
    const headers = {
      'Content-Type' : 'application/json',
      'Accept': 'application/json; charset=utf-8',
    };
    const response = await fetch(url, {
        method: 'POST',
        headers,
        body : JSON.stringify({username, password})
    });
    if (response.status === 204) {
        return true;
    }
    else {
      await processError(response, errorCallback);
      return false;
    }
}

export async function logout(errorCallback? : RequestErrorCallback) : Promise<boolean>
{
    const url = 'api/account/logout'
    const headers = {
      'Content-Type' : 'application/json',
      'Accept': 'application/json; charset=utf-8',
    };
    const response = await fetch(url, {
        method: 'POST',
        headers
    });
    if (response.status === 204) {
        return true;
    }
    else {
      await processError(response, errorCallback);
      return false;
    }
}


async function processError(response : Response, errorCallback? : RequestErrorCallback) {
  if (errorCallback !== undefined) {
    if (response.status === 400 || response.status === 500) {
        const problemDetails : any = await response.json();
        errorCallback(response.status, problemDetails.title ?? '', problemDetails.detail ?? '', problemDetails.errors ?? {});
    }
    else {
        errorCallback(response.status, response.statusText ?? '', await response.text() ?? '',  {});
    }
  }
}
