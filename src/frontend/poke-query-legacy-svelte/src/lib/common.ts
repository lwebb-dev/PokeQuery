export const statNames = {
    hp: "HP",
    attack: "Attack",
    defense: "Defense",
    "special-attack": "Sp. Atk",
    "special-defense": "Sp. Def",
    speed: "Speed" 
};

// Expected Url Format: /api/v2/{prefix}/{id}/
export const getIdFromUrl = (url: string): number => {
    return parseInt(url.split("/")[4]);
}

// Expected Url Format: /api/v2/{prefix}/{id}/
export const getPrefixFromUrl = (url: string): number => {
    return parseInt(url.split("/")[3]);
}

export const fetchJson = async (baseUri: string, prefix: string, id?: number): Promise<any> => {
    let idPath = typeof (id) !== "undefined" ? `/${id}` : "";
    const response = await fetch(`${baseUri}/${prefix}${idPath}`);
    return await response.json();
}

export const fetchJsonString = async (baseUri: string, prefix: string, id?: number): Promise<string> => {
    const json = await fetchJson(baseUri, prefix, id);
    return JSON.stringify(json);
}