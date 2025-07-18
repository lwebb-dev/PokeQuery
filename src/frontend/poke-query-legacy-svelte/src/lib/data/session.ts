import { fetchJsonString } from "../common";

export let isLoadingSessionData = false;

let BASE_URI: string = '';

export const loadSessionData = async (baseUri) => {
  if (
    typeof sessionStorage.typeData !== "undefined" 
    && typeof sessionStorage.versionGroupData !== "undefined" 
    && typeof sessionStorage.machineData !== "undefined"
  ) {
    return Promise.resolve();
  }

  BASE_URI = baseUri; 
  isLoadingSessionData = true;

  return Promise.all([
    sessionStorage.typeData = await fetchJsonString(BASE_URI, "type"),
    sessionStorage.versionGroupData = await fetchJsonString(BASE_URI, "version-group"),
    sessionStorage.machineData = await fetchJsonString(BASE_URI, "machine")
  ]).finally(() => {
    isLoadingSessionData = false;
  });
};
