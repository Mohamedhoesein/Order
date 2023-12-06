import fs from "fs";
import path from "path";

export interface LastEmailArguments {
    name: string;
    email: string;
}

export interface LastEmail {
    type: 'verify' | 'change-password' | 'email' | 'unknown';
    id: string;
    code: string;
}

export const getLastEmail = (info: LastEmailArguments): LastEmail => {
    const data = lastEmailFile(info);

    if (data === '') {
        const basePath = './src/cypress/email/';
        const userFolder = info.name + '.' + info.email.replace('@', '_at_') + '/';
        const l = basePath + userFolder;
        return {
            type: 'unknown',
            id: path.resolve(l),
            code: '' + fs.existsSync(l)
        }
    }

    return processEmail(data);
}

const delay = (ms: number): void => {
    const date = Date.now();
    let currentDate = null;

    do {
        currentDate = Date.now();
    } while (currentDate - date < ms);
}


const lastEmailFile = ({name, email}: LastEmailArguments): string => {
    const basePath = './src/cypress/email/';
    const userFolder = name + '.' + email.replace('@', '_at_') + '/';
    const path = basePath + userFolder;
    waitForExist(path, 4000);

    if (fs.existsSync(path))
    {
        delay(2000);
        const parts = fs.readdirSync(path);
        if (parts.length == 0)
            return '';
        const file = parts.map(name => {
            return {
                name,
                time: fs.statSync(path).mtime
            }
        }).sort((a, b) => {
            if (a.time > b.time) {
                return -1;
            }
            else if (a.time < b.time) {
                return 1;
            }
            else {
                return 0;
            }
        })[0];
        const fileContent = fs.readFileSync(path + file.name).toString();

        fs.unlinkSync(path + file.name);

        return fileContent;
    }

    return '';
}

const waitForExist = (path: string, ms: number): void => {
    const date = Date.now();
    let currentDate = null;

    do {
        currentDate = Date.now();
    } while (!fs.existsSync(path) && currentDate - date < ms);
}

const processEmail = (email: string): LastEmail => {
    let lines: string[];
    if (email.includes('\r\n'))
        lines = email.split('\r\n');
    else
        lines = email.split('\n');

    lines = lines.filter(line => line.trim() !== '');

    const split_content = lines[lines.length - 1].split(' ');
    const split_email = split_content[split_content.length - 1].split('/')

    const type = split_email[split_email.length - 3].trim();
    const id = split_email[split_email.length - 2].trim();
    const code = split_email[split_email.length - 1].trim();
    if (type === 'verify' || type === 'change-password' || type === 'email') {
        return {
            type,
            id,
            code
        }
    }

    return {
        type: 'unknown',
        id: `type:${type}======id:${id}======code:${code}`,
        code: ''
    }
}

