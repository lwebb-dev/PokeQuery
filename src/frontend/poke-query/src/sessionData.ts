// sessionData.ts
// Utility to load typeData, versionGroupData, and machineData into sessionStorage (like Svelte)

const fetchJsonString = async (baseUri: string, endpoint: string) => {
  const res = await fetch(`${baseUri}/${endpoint}`);
  if (!res.ok) throw new Error(`Failed to fetch ${endpoint}`);
  return JSON.stringify(await res.json());
};

export const loadSessionData = async (baseUri: string) => {
  console.log('loadSessionData called with baseUri:', baseUri);

  if (
    sessionStorage.getItem('typeData') &&
    sessionStorage.getItem('versionGroupData') &&
    sessionStorage.getItem('machineData')
  ) {
    console.log('Session data already loaded, skipping fetch');
    return;
  }

  try {
    console.log('Fetching session data...');
    sessionStorage.setItem('typeData', await fetchJsonString(baseUri, 'type'));
    sessionStorage.setItem('versionGroupData', await fetchJsonString(baseUri, 'version-group'));
    sessionStorage.setItem('machineData', await fetchJsonString(baseUri, 'machine'));
    console.log('Session data loaded successfully');
  } catch (error) {
    console.error('Failed to load session data:', error);
  }
};
