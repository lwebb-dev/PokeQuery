// sessionData.ts
// Utility to load typeData, versionGroupData, and machineData into sessionStorage (like Svelte)

const fetchJsonString = async (baseUri: string, endpoint: string) => {
  const res = await fetch(`${baseUri}/${endpoint}`);
  if (!res.ok) throw new Error(`Failed to fetch ${endpoint}`);
  return JSON.stringify(await res.json());
};

export const loadSessionData = async (baseUri: string) => {
  if (
    typeof sessionStorage.typeData !== 'undefined' &&
    typeof sessionStorage.versionGroupData !== 'undefined' &&
    typeof sessionStorage.machineData !== 'undefined'
  ) {
    return;
  }
  sessionStorage.typeData = await fetchJsonString(baseUri, 'type');
  sessionStorage.versionGroupData = await fetchJsonString(baseUri, 'version-group');
  sessionStorage.machineData = await fetchJsonString(baseUri, 'machine');
};
