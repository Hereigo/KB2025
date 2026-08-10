const ERROR_MESSAGE = 'Not unique login';

export class NotUniqueLoginError extends Error {
    constructor () {
        super(ERROR_MESSAGE);

        this.statusCode = 400;
    }
}
