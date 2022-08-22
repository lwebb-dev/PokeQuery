export let isLoadingSessionData = false;

let BASE_URI: string = '';

export const loadSessionData = async (baseUri) => {
  if (
    typeof sessionStorage.typeData !== "undefined" 
    && typeof sessionStorage.versionGroupData !== "undefined" 
    && typeof sessionStorage.machineData !== "undefined"
    // && typeof sessionStorage.generationData !== "undefined"
  ) {
    return Promise.resolve();
  }

  BASE_URI = baseUri; 
  isLoadingSessionData = true;

  return Promise.all([
    loadTypeDataIntoSession(),
    loadVersionGroupDataIntoSession(),
    loadMachineDataIntoSession(),
    // loadGenerationDataIntoSession(baseUri),
  ]).finally(() => {
    isLoadingSessionData = false;
  });
};

const loadTypeDataIntoSession = async () => {
  await fetch(`${BASE_URI}/types`, {
    method: "GET",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },
  })
    .then((r) => {
      if (!r.ok) {
        throw new Error("API FAILED TO RETURN 200 OK ON /types");
      }
      return r.json();
    })
    .then((data) => {
      sessionStorage.typeData = JSON.stringify(data);

    });
};

const loadVersionGroupDataIntoSession = async () => {
  await fetch(`${BASE_URI}/version-groups`, {
    method: "GET",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },
  })
    .then((r) => {
      if (!r.ok) {
        throw new Error("API FAILED TO RETURN 200 OK ON /version-groups");
      }
      return r.json();
    })
    .then((data) => {
      sessionStorage.versionGroupData = JSON.stringify(data);

    });
};

const loadMachineDataIntoSession = async () => {
  await fetch(`${BASE_URI}/machines`, {
    method: "GET",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },
  })
    .then((r) => {
      if (!r.ok) {
        throw new Error("API FAILED TO RETURN 200 OK ON /version-groups");
      }
      return r.json();
    })
    .then((data) => {
      sessionStorage.machineData = JSON.stringify(data);

    });
};


// const loadGenerationDataIntoSession = async () => {
//   await fetch(`${BASE_URI}/generations`, {
//     method: "GET",
//     headers: {
//       Accept: "application/json",
//       "Content-Type": "application/json",
//     },
//   })
//     .then((r) => {
//       if (!r.ok) {
//         throw new Error("API FAILED TO RETURN 200 OK ON /generations");
//       }
//       return r.json();
//     })
//     .then((data) => {
//       sessionStorage.generationData = JSON.stringify(data);
//     });
// };
