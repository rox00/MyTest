import path from 'path'
import { fileURLToPath } from 'url'


export default class BNMSClientConfig {
    constructor() {
        const __filenameNew = fileURLToPath(import.meta.url)
        const __dirnameNew = path.dirname(__filenameNew)
        var PROTO_PATH = __dirnameNew + '/BNMSStatus.proto';

        this.servername = 'localhost';
        this.serverport = '50051';
        this.protofile = PROTO_PATH;
      }
}