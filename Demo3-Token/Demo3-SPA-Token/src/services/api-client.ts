import { getJwtToken, logout, storeToken } from "./token-util";

export type RequestErrorCallback = (status : number, title:string, detail : string, error: {[key: string] : string}) => void;

const api = 'https://localhost:7083'
export async function fetchItems<T>(endpoint : string, errorCallback? :  RequestErrorCallback) : Promise<T[]>
{
    const token = getJwtToken();
    const headers = {
      'Content-Type' : 'application/json',
      'Accept': 'application/json; charset=utf-8',
      'Authorization' : `Bearer ${token}`
    };

    const response = await fetch(`${api}/${endpoint}`, { headers });
    if (response.ok) {
        const data = await response.json() as T[];
        return data;
    }
    else {
      if (response.status === 401) {
        logout()
      }
      await processError(response, errorCallback)
      return []
    }
}

export async function login(username : string, password : string, errorCallback? : RequestErrorCallback) : Promise<boolean>
{
    const url =`${api}/account/login`
    const headers = {
      'Content-Type' : 'application/json',
      'Accept': 'application/json; charset=utf-8',
    };
    const response = await fetch(url, {
        method: 'POST',
        headers,
        body : JSON.stringify({username, password})
    });
    if (response.status === 200) {
        const token = await response.text();
        storeToken(token);
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
