export function WMBISetCurrentLanguage(lang, storageType) {
    if (storageType === "localStorage") {
        window.localStorage.setItem('wmbi-language', lang);
    } else if (storageType === "sessionStorage") {
        window.sessionStorage.setItem('wmbi-language', lang);
    }
}

export function WMBIGetCurrentLanguage(storageType) {
    if (storageType === "localStorage") {
        return window.localStorage.getItem('wmbi-language');
    } else if (storageType === "sessionStorage") {
        return window.sessionStorage.getItem('wmbi-language');
    } else {
        return null;
    }
}