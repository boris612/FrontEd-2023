import { authPlugin } from "src/boot/auth0";
const { getAccessTokenSilently, isAuthenticated   } = authPlugin

export type RequestErrorCallback = (status : number, title:string, detail : string, error: {[key: string] : string}) => void;



const api = 'https://localhost:7284'
export async function fetchItems<T>(endpoint : string, errorCallback? :  RequestErrorCallback) : Promise<T[]>
{


    const headers : Record<string, string> = {
      'Content-Type' : 'application/json',
      'Accept': 'application/json; charset=utf-8',
    };

    if (isAuthenticated) {
      const accessToken = await getAccessTokenSilently()
      headers.Authorization = `Bearer ${accessToken}`
    }


    const response = await fetch(`${api}/${endpoint}`, { headers });
    if (response.ok) {
        const data = await response.json() as T[];
        return data;
    }
    else {
      await processError(response, errorCallback)
      return []
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
