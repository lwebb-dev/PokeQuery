export let isLoadingSessionData = false;

export const loadSessionData = async (baseUri) => {
  if (
    typeof sessionStorage.typeData !== "undefined" 
    && typeof sessionStorage.versionGroupData !== "undefined" 
    // && typeof sessionStorage.generationData !== "undefined"
  ) {
    return Promise.resolve();
  }

  isLoadingSessionData = true;

  return Promise.all([
    loadTypeDataIntoSession(baseUri),
    loadVersionGroupDataIntoSession(baseUri),
    // loadGenerationDataIntoSession(baseUri),
  ]).finally(() => {
    isLoadingSessionData = false;
  });
};

const loadTypeDataIntoSession = async (baseUri) => {
  await fetch(`${baseUri}/types`, {
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
      sessionStorage.typeData = data;

    });
};

const loadVersionGroupDataIntoSession = async (baseUri) => {
  await fetch(`${baseUri}/version-groups`, {
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
      sessionStorage.versionGroupData = data;

    });
};

// const loadGenerationDataIntoSession = async (baseUri) => {
//   await fetch(`${baseUri}/generations`, {
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
