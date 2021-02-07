export function WMBISetCurrentLanguage(lang) {
    window.localStorage.setItem('wmbi-language', lang);
}

export function WMBIGetCurrentLanguage() {
    return window.localStorage.getItem('wmbi-language');
}