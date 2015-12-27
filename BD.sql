/*BASE DE DATOS DEL SISTEMA JUNTO CON UN PAR DE TRIGGERS QUE SIRVEN COMO MECANISMOS AUTOMATICOS EN CASO DE ELIMINARSE CIERTOS REGISTROS ENCADENADOS DE LA BD (VER NORMALIZACION)*/
CREATE DATABASE servicioTecnico
USE servicioTecnico

CREATE TABLE Tipos
(
idTipo smallint IDENTITY (1,1),
tipo nvarchar(15) NOT NULL
CONSTRAINT pkTipo PRIMARY KEY (idTipo)
)

CREATE TABLE Marcas
(
idMarca smallint IDENTITY (1,1),
marca nvarchar(15) NOT NULL
CONSTRAINT pkMarca PRIMARY KEY (idMarca)
)

CREATE TABLE Barrios
(
idBarrio smallint IDENTITY (1,1),
barrio nvarchar(30) NOT NULL
CONSTRAINT pkBarrio PRIMARY KEY (idBarrio)
)

CREATE TABLE Roles
(
idRol smallint IDENTITY (1,1),
rol nvarchar(15) NOT NULL,
CONSTRAINT pkRol PRIMARY KEY (idRol)
)

CREATE TABLE TipoMarca
(
idTipoMarca smallint IDENTITY (1,1),
idTipo smallint,
idMarca smallint
CONSTRAINT pkTipoMarca PRIMARY KEY (idTipoMarca),
CONSTRAINT fkTipo FOREIGN KEY (idTipo) REFERENCES Tipos (idTipo),
CONSTRAINT fkMarca FOREIGN KEY (idMarca) REFERENCES Marcas (idMarca)
)

CREATE TABLE Clientes
(
idClie smallint IDENTITY(1,1),
nomClie nvarchar(20),
apeClie nvarchar(20),
direClie nvarchar(40),
idBarrio smallint,
telClie nvarchar(15)
CONSTRAINT pkCliente PRIMARY KEY (idClie),
CONSTRAINT fkBarrio FOREIGN KEY (idBarrio) REFERENCES Barrios (idBarrio)
)

CREATE TABLE Usuarios
(
idUsu smallint IDENTITY (1,1),
nomUsu nvarchar(40),
direcUsu nvarchar(40),
idBarrio smallint,
telUsu nvarchar(15),
nomLog nvarchar (10),
passLog char (10),
idRol smallint,
habilitar bit
CONSTRAINT pkUsuario PRIMARY KEY (idUsu),
CONSTRAINT fkRol FOREIGN KEY (idRol) REFERENCES Roles (idRol),
CONSTRAINT fkBarrioU FOREIGN KEY (idBarrio) REFERENCES Barrios (idBarrio)
)

CREATE TABLE Equipos
(
idEquipo int IDENTITY (1,1),
idTipoMarca smallint,
modelo nvarchar(20),
numSerie nvarchar(30),
idClie smallint
CONSTRAINT pkEquipo PRIMARY KEY (idEquipo),
CONSTRAINT fkTipoMarca FOREIGN KEY (idTipoMarca) REFERENCES TipoMarca (idTipoMarca),
CONSTRAINT fkCliente FOREIGN KEY (idClie) REFERENCES Clientes (idClie)
)

CREATE TABLE Estados
(
idEstado smallint IDENTITY(1,1),
estado nvarchar(14)
CONSTRAINT pkEstado PRIMARY KEY (idEstado)
)

CREATE TABLE Ordenes
(
idOrden int IDENTITY(1,1),
idEquipo int,
falla nvarchar(150),
observ nvarchar(150),
fecRecib smalldatetime,
idEstado smallint,
aviso bit,
fecAviso smalldatetime,
fecEntrega smalldatetime
CONSTRAINT pkOrden PRIMARY KEY (idOrden),
CONSTRAINT fkEquipo FOREIGN KEY (idEquipo) REFERENCES Equipos (idEquipo),
CONSTRAINT fkEstados FOREIGN KEY (idEstado) REFERENCES Estados (idEstado)
)

CREATE TABLE Tareas
(
idTarea int IDENTITY (1,1),
idOrden int,
detalles nvarchar(300),
fecTarea smalldatetime,
idUsu smallint
CONSTRAINT pfTarea PRIMARY KEY (idTarea),
CONSTRAINT fkOrden FOREIGN KEY (idOrden) REFERENCES Ordenes (idOrden),
CONSTRAINT fkTecnico FOREIGN KEY (idUsu) REFERENCES Usuarios (idUsu)
)

INSERT INTO Roles (rol) VALUES ('Administrador');
INSERT INTO Roles (rol) VALUES ('Recepcionista');
INSERT INTO Roles (rol) VALUES ('Tecnico');

INSERT INTO Barrios (barrio) VALUES ('---');
INSERT INTO barrios (barrio) VALUES ('1° de Mayo')
INSERT INTO barrios (barrio) VALUES ('Acosta')
INSERT INTO barrios (barrio) VALUES ('Alberdi')
INSERT INTO barrios (barrio) VALUES ('Alta Córdoba')
INSERT INTO barrios (barrio) VALUES ('Altamira')
INSERT INTO barrios (barrio) VALUES ('Alto Alberdi')
INSERT INTO barrios (barrio) VALUES ('Alto General Paz')
INSERT INTO barrios (barrio) VALUES ('Alto Verde')
INSERT INTO barrios (barrio) VALUES ('Altos de Velez Sarsfield')
INSERT INTO barrios (barrio) VALUES ('Ampliacion America')
INSERT INTO barrios (barrio) VALUES ('Ayacucho')
INSERT INTO barrios (barrio) VALUES ('Cofico')
INSERT INTO barrios (barrio) VALUES ('Colón')
INSERT INTO barrios (barrio) VALUES ('Colonia Lola')
INSERT INTO barrios (barrio) VALUES ('Comercial')
INSERT INTO barrios (barrio) VALUES ('Crisol Norte')
INSERT INTO barrios (barrio) VALUES ('Crisol Sur')
INSERT INTO barrios (barrio) VALUES ('Empalme')
INSERT INTO barrios (barrio) VALUES ('Ferreyra')
INSERT INTO barrios (barrio) VALUES ('General Bustos')
INSERT INTO barrios (barrio) VALUES ('General Mosconi')
INSERT INTO barrios (barrio) VALUES ('General Paz')
INSERT INTO barrios (barrio) VALUES ('Ituzaingo')
INSERT INTO barrios (barrio) VALUES ('Jardin')
INSERT INTO barrios (barrio) VALUES ('Jorge Newbery')
INSERT INTO barrios (barrio) VALUES ('Liceo General Paz')
INSERT INTO barrios (barrio) VALUES ('Los Boulevares')
INSERT INTO barrios (barrio) VALUES ('Los Gigantes')
INSERT INTO barrios (barrio) VALUES ('Los Pinos')
INSERT INTO barrios (barrio) VALUES ('Maipu')
INSERT INTO barrios (barrio) VALUES ('Marqués de Sobremonte')
INSERT INTO barrios (barrio) VALUES ('Matienzo')
INSERT INTO barrios (barrio) VALUES ('Nueva Córdoba')
INSERT INTO barrios (barrio) VALUES ('Nueva Italia')
INSERT INTO barrios (barrio) VALUES ('Patricios')
INSERT INTO barrios (barrio) VALUES ('Pueyrredon')
INSERT INTO barrios (barrio) VALUES ('Residencial America')
INSERT INTO barrios (barrio) VALUES ('Rosedal')
INSERT INTO barrios (barrio) VALUES ('San Martin')
INSERT INTO barrios (barrio) VALUES ('San Roque')
INSERT INTO barrios (barrio) VALUES ('San Vicente')
INSERT INTO barrios (barrio) VALUES ('Santa Isabel')
INSERT INTO barrios (barrio) VALUES ('Sarmiento')
INSERT INTO barrios (barrio) VALUES ('Villa Libertador')
INSERT INTO barrios (barrio) VALUES ('Yapeyú')

INSERT INTO Tipos (tipo) VALUES ('Equipo');
INSERT INTO Tipos (tipo) VALUES ('Celular');
INSERT INTO Tipos (tipo) VALUES ('Tablet');
INSERT INTO Tipos (tipo) VALUES ('Pc Escritorio');
INSERT INTO Tipos (tipo) VALUES ('Notebook');
INSERT INTO Tipos (tipo) VALUES ('Netbook');

INSERT INTO Marcas (marca) VALUES ('Generico');
INSERT INTO Marcas (marca) VALUES ('Bangho');
INSERT INTO Marcas (marca) VALUES ('Samsung');
INSERT INTO Marcas (marca) VALUES ('Sony');
INSERT INTO Marcas (marca) VALUES ('HP');
INSERT INTO Marcas (marca) VALUES ('Acer');
INSERT INTO Marcas (marca) VALUES ('Blackberry');
INSERT INTO Marcas (marca) VALUES ('Nokia');
INSERT INTO Marcas (marca) VALUES ('Apple');
INSERT INTO Marcas (marca) VALUES ('Compaq');
INSERT INTO Marcas (marca) VALUES ('Alcatel');
INSERT INTO Marcas (marca) VALUES ('Motorola');
INSERT INTO Marcas (marca) VALUES ('Dell');
INSERT INTO Marcas (marca) VALUES ('LG');
INSERT INTO Marcas (marca) VALUES ('Asus');
INSERT INTO Marcas (marca) VALUES ('Lenovo');

INSERT INTO TipoMarca (idTipo, idMarca) VALUES (1,1);--EQUIPO GENERICO/A - 1
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (4,1);--PC ESCRITORIO GENERICO/A - 2
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (4,2);--PC ESCRITORIO BANGHO - 3
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (5,2);--NOTEBOOK BANGHO - 4
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (6,2);--NETBOOK BANGHO - 5
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (2,3);--CELULAR SAMSUNG - 6
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (3,3);--TABLET SAMSUNG - 7
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (5,3);--NOTEBOOK SAMSUNG - 8
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (2,4);--CELULAR SONY - 9
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (3,4);--TABLET SONY - 10
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (5,4);--NOTEBOOK SONY - 11
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (5,5);--NOTEBOOK HP - 12
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (6,5);--NETBOOK HP - 13
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (6,6);--NETBOOK ACER - 14
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (2,7);--CELULAR BLACKBERRY - 15
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (2,8);--CELULAR NOKIA - 16
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (3,9);--TABLET APPLE - 17
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (4,10);--PC ESCRITORIO COMPAQ - 18
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (5,10);--NOTEBOOK COMPAQ - 19
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (6,10);--NETBOOK COMPAQ - 20
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (2,11);--CELULAR ALCATEL - 21
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (2,12);--CELULAR MOTOROLA - 22
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (4,13);--PC ESCRITORIO DELL - 23
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (5,13);--NOTEBOOK DELL - 24
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (2,14);--CELULAR LG - 25
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (2,15);--CELULAR ASUS - 26
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (3,15);--TABLET ASUS - 27
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (5,15);--NOTEBOOK ASUS - 28
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (6,15);--NETBOOK ASUS - 29
INSERT INTO TipoMarca (idTipo, idMarca) VALUES (5,16);--NOTEBOOK LENOVO - 30

INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Gisela', 'Durando', 'Tucuman 2081', 3, '351-158872568');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Claudio', 'Araoz', 'Ramos Mejia 210', 4, '3541-575182');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Cistina', 'Gutierrez', 'Juan Segui 2248', 5, '3541-550643');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Adriana', 'Quaranta', 'Resistencia 151', 3, '3541-233893');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Mauricio Ezequiel', 'Gomez', 'Los Tamarindos 25',7 , '3541-658418');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Denise', 'Aviles Pardavila', 'Cerro Blanco 669', 8, '3541-667407');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Emmanuel Alberto', 'Metrebian', 'Genaral Paz 685', 9, '3541-587613');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Angela Lucia', 'Passerino', 'Tucuman 281', 9, '351-6665948');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Ernesto', 'Tortolo', 'Irigoyen esq. Nicolas Avellaneda', 9, '3541-543032');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Franco', 'Plavnik', 'Roque Saenz Peña 668', 12, '3541-680111');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Gustavo', 'Rossito', 'Martin Luther King 20', 13, '3541-534675');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Susana', 'Maldonado', 'Derqui 329', 14, '3541-649484');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Ailen', 'Bombelli', 'Rivadavia 475', 16, '3541-523777');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Pablo', 'Chirino', 'Delqui 300', 16, '3541-331496');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Juan Nicolas', 'Osses', 'Alberto Durero 67', 15, '3541-681698');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Axel Catriel', 'Marchisio', 'Cuyo 117', 18, '3541-598106');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Ariel', 'Aguilar', 'Gutemberg 614', 19, '3541-561461');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Eugenia', 'Stabio', 'Sarasate 588', 5, '351-6628232');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Nilda', 'Ormaechea', 'Córdoba 215', 6, '3541-53250');
INSERT INTO Clientes (nomClie, apeClie, direClie, idBarrio, telClie) VALUES ('Emmanuel', 'Den Dauw', 'Mal Paso 1350', 7, '356-4697607');

INSERT INTO Usuarios (nomUsu, telUsu, direcUsu, idBarrio, nomLog, passLog, idRol,habilitar) VALUES ('Nicolas Gonzalez', '3541-15570931', 'Tucuman 281', 2, 'admin', 'adminadmin', 1, 1);
INSERT INTO Usuarios (nomUsu, telUsu, direcUsu, idBarrio, nomLog, passLog, idRol, habilitar) VALUES ('Marilu Gutierrez', '351-15665470', 'Gob. Ferreyra 520', 6, 'mgutierrez', 'marilu1992', 2, 1);
INSERT INTO Usuarios (nomUsu, telUsu, direcUsu, idBarrio, nomLog, passLog, idRol, habilitar) VALUES ('Carolina Gutierrez', '3541-1576705', 'Buenos Aires esq. Derqui', 5, 'cgutierrez', 'carolina14', 2, 1);
INSERT INTO Usuarios (nomUsu, telUsu, direcUsu, idBarrio, nomLog, passLog, idRol, habilitar) VALUES ('Fernando Libossart', '3541-1576693', 'Gob. Ferreyra 520', 6, 'flibossart', 'gallego500', 3, 1);
INSERT INTO Usuarios (nomUsu, telUsu, direcUsu, idBarrio, nomLog, passLog, idRol, habilitar) VALUES ('Leonardo Zuliani', '351-15953861', 'Los Tamarindos 250', 6, 'lzuliani', 'leonar2014', 3, 1);

INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (4, 'I1-458', '1-0004581', 1);--1
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (2, 'CX', 'NINGUNO', 1);--2
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (4, 'i2-511', '1-0005152', 2);--3
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (4, 'B251xhu', '1-0000251', 2);--4
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (2, 'SFX', 'NINGUNO', 3);--5
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (6, 'GALAXY TREND LITE', '156684356874161', 3);--6
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (8, 'NP-270E5E', '354958215736598', 4);--7
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (9, 'XPERIA L', '165488526969441', 5);--8
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (9, 'XPERIA L', '158426698522491', 6);--9
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (12, 'ELITEBOOK 8470P', 'CNE3654E7', 6);--10
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (7, 'GALAXY NOTE 7', 'WME253HMBLA', 7);--11
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (14, 'V3-571G-6800', '715061489159', 7);--12
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (15, 'CURVE 9320', '165489524578951', 8);--13
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (30, 'G480', '351687417415', 9);--14
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (16, 'C3', '132469875125891', 9);--15
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (2, 'SENTEY', 'NINGUNO', 10);--16
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (19, 'IPHONE 4S', '163212875132251', 11);--17
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (19, 'IPHONE 4', '162468152145691', 12);--18
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (21, 'IDOL MINI', '163168422598741', 13);--19
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (22, 'MOTO G', '14692158	4852161', 14);--20
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (24, 'INSPIRON 3421', '2469843698ALE', 15);--21
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (25, 'G2', '123438715851241', 16);--22
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (28, 'N56V', '75L0A245585', 16);--23
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (25, 'G2', '162487569596211', 17);--24
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (6, 'S4 MINI', '136487541812561', 18);--25
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (11, 'VAIO SVF14214', 'N2154AS1356LA', 19);--26
INSERT INTO Equipos (idTipoMarca, modelo, numSerie, idClie) VALUES (22, 'RAZR D1', '131643285297641', 20);--27

INSERT INTO Estados (estado) VALUES ('En Reparacion');
INSERT INTO Estados (estado) VALUES ('Reparada');
INSERT INTO Estados (estado) VALUES ('Sin Solucion');

--ORDEN 1
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (1, 'se apaga a los 10 minutos de uso', 'deja cargador original y funda color rosa', '03/11/2014', 2, 1, '05/11/2014', '07/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (1, 'se realizan pruebas por 2 horas, se apaga varias veces con temperatura superior a los 80°', '03/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (1, 'se desarma la notebook entera y se limpia el disipador, ademas se cambia la pasta termica del procesador', '03/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (1, 'se deja probando durante 22 horas la computadora, la temperatura no supera los 45°, se realizan pruebas de disco duro y memoria para finalizar analisis, todo ok', '5/11/2014', 5);
--ORDEN 2
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (3, 'anda lento el sistema', 'deja cargador original', '04/11/2014', 2, 1, '06/11/2014', '07/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (2, 'se prueba el sistema, no parece haber problemas de disco, se analiza el mismo para descartar posibilidades', '04/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (2, 'disco ok, se aconseja reinstalar directamente el sistema, se espera confirmacion del cliente', '05/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (2, 'se confirma reinstalacion del sistema operativo con resguardo de datos', '05/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (2, 'se finaliza el trabajo, se configura y prueba por ultima vez el sistema, ninguna falla', '06/11/2014', 5);
--ORDEN 3
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (5, 'sistema lento, hace mucho ruido al jugar juegos de facebook', 'ninguno', '05/11/2014', 2, 1, '10/11/2014', '13/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (3, 'se prueba el equipo y se detecta lentitud en carga de paginas, parece llena de programas secundarios, se procede a optimizar el sistema', '06/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (3, 'se realiza optimizacion completa (drivers, autoruns, ccleaner, desgragmentar), se espera que termine de desfragmentar para ver hardware interno', '07/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (3, 'se ve interior del equipo, se acomodan cables del interior y limpia polvo del equipo, no se cambio pasta termica, trabajo finalizado', '07/11/2014', 4);
--ORDEN 4
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (7, 'no da imagen la pantalla, prende correctamente pero no da imagen la pantalla', 'deja cargador original y funda de hello kity azul', '06/11/2014', 2, 1, '13/11/2014', '14/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (4, 'se prende equipo y se conecta la entrada vga a un monitor de trabajo, da imagen, se hace presupuesto para cambio de pantalla', '06/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (4, 'presupuesto aceptado y repuesto recibido, se hace cambio de pantalla, muestra imagen, se deja 1 dia prendido para chequear hardware', '12/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (4, 'hardware chequeado, temperaturas OK, pantalla funcionando, equipo finalizado', '13/11/2014', 4);
--ORDEN 5
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (8, 'se mojo y no prende', 'deja cargador original, no deja funda', '07/11/2014', 3, 1, '10/11/2014', '16/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (5, 'se desarme el equipo y hace baño quimico a la placa, se rearma y se prueba, sigue fallando', '07/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (5, 'se prueba con un segundo baño quimico, cepillado de conectores y cambio de pin de carga, sigue fallando', '08/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (5, 'se prueba con 2 baterias originales y sigue sin encender, caso desestimado', '10/11/2014', 4);
--ORDEN 6
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (9, 'se reinicia varias veces el sistema, bateria dura poco', 'ninguno', '08/11/2014', 2, 1, '12/11/2014', '14/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (6, 'se hare restauracion de sistema con respaldo de datos en memoria interna y contactos', '08/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (6, 'se actualiza el sistema a ultima version disponible, se instala whatsapp y aplicaciones basicas que pidio el usuario, se nota bateria inchada, se pide bateria nueva', '10/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (6, 'se prueba telefono con bateria nueva por el dia entero, finaliza con un consumo aceptable de bateria, termino siendo la anterior que estaba ya hinchada, trabajo finalizado', '11/11/2014', 5);
--ORDEN 7
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (11, 'pantalla partida', 'cargador original y funda sin marca color amarillo', '10/11/2014', 2, 1, '15/11/2014', '19/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (7, 'se pidio nueva pantalla y llego al local, se pudo colocar sin problemas', '14/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (7, 'se hacen prueba de funcionamiento por 1 dia entero, no da problemas la pantalla instalada, trabajo resuelto', '15/11/2014', 5);
--ORDEN 8
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (13, 'no se puede escuchar nada, ni llamados ni musica', 'deja cargador original', '11/11/2014', 2, 1, '15/11/2014', '18/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (8, 'se prueba sonido por salida auricular, funciona, no es problema del chip de audio, se pide cambio de parlante interno', '11/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (8, 'llego el repuesto interno y se cambia, funciona sin problema, se lo deja reproduciendo musica por el resto del dia', '14/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (8, 'pasa la prueba de sonido mantenido por el dia seguido, trabajo finalizado', '15/11/2014', 4);
--ORDEN 9
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (14, 'sistema clavado, no se puede ingresar a windows, posibles virus', 'cargador original con funda ferrari roja, en caso de formatear avisemos al cliente', '12/11/2014', 2, 1, '20/11/2014', '21/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (9, 'se intenta ingresar con el mini xp del hiren, al ir cargando se escuchan clacs metalicos, pasa hd tune al disco, muy dañado, pedir cambio y recuperacion de datos', '13/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (9, 'llego el disco un rato antes de correr, solo se pudo pasar hd tune para evitar sospechas, todo ok', '18/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (9, 'se instala windows con aplicaciones y backup cargado, se hacen pruebas para asegurar estabilidad del sistema, trabajo finalizado', '20/11/2014', 5);
--ORDEN 10
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (16, 'no inicia windows, se reinicia constantemente al aparecer el logo del mismo', 'en caso de formatear pide windows 7, fijarse cantidad de ram', '13/11/2014', 2, 1, '18/11/2014', '21/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (10, 'se fija pantalla azul al iniciarse windows, problema del disco duro, hacer analisis del mismo con hd tune a ver daño', '13/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (10, 'analisis de disco muestra daño en varios sectores repartidos por varias zonas del disco, no se aconseja reutilizar disco directamente, se envia aviso de presupuesto para disco nuevo', '13/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (10, 'presupuesto aceptado, llego el disco y se procede a instalar windows y hacer recuperacion de informacion del disco anterior', '17/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (10, 'se instalo correctamente windows con programas y drivers actualizados y backup ya colocado, se deja en prueba por el resto del dia para probar estabilidad', '18/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (10, 'la pc pasa la prueba de estabilidad, todo ok, trabajo finalizado', '18/11/2014', 5);
--ORDEN 11
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (17, 'actualizo software de sistema y no agarra señal de celular ni internet', 'celular liberado', '14/11/2014', 2, 1, '18/11/2014', '20/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (11, 'se prueba con chips de otras compañias, ninguno funciona', '14/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (11, 'se hace busqueda por imei para saber estado del celular, no es banda negativa', '14/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (11, 'se intenta reinstalar el SO de fabrica con software de fabricante, no permite reinstalacion, sale error 0x021458714f, el error es a causa de un daño en el SO del celular', '15/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (11, 'se prueba reistalar el SO del celular con una herramienta alternativa, reinstala correctamente pero sigue sin señal', '17/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (11, 'se prueba reinstalar el SO del celular con la herramienta del fabricante por segunda vez, se pudo reinstalar y agarra señal, se restaura backup de contactos y documentos, trabajo finalizado', '18/11/2014', 4);
--ORDEN 12
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (18, 'dejo de funcionar el tactil', 'deja cargador original', '15/11/2014', 2, 1, '20/11/2014', '21/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (12, 'se desarma el celular y se prueba la pantalla, no tiene daños internos, se pide repuesto de tactil solamente, al parecer el original esta cortado', '15/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (12, 'se recibe el repuesto de tactil y se rensambla al celular, se prueba por 10 minutos, todas las funciones tactiles ok, trabajo finalizado', '20/11/2014', 5);
--ORDEN 13
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (19, 'se cayo al suelo y desde ese momento no puede hablar ni escuchar correctamente', 'deja cargador original', '17/11/2014', 3, 1, '02/12/2014', '05/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (13, 'se prueba hablar, directamente no funciona el microfono ni el parlante, se prueba musica y no se escucha, se piden repuestos de parlante y microfono', '17/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (13, 'llegan repuestos de parlante y microfono y se colocan al celular, se hace prueba de sonido y sigue sin funcionar, se pide un segundo par de repuestos', '22/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (13, 'llega el segundo par de repuestos y se instalan al celular, sigue con la misma falla que el principio, fijarse placa por falla en las pistas', '29/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (13, 'se hace inspeccion a la placa del equiopo en busca de rotura o daño por la caida, el chip de audio presenta una rotura seria pero no se puede cambiar, se recomienda al cliente un servicio apple autorizado para solucionar el problema, trabajo sin solucion', '02/11/2014', 4);
--ORDEN 14
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (20, 'se cayo a una pileta, no enciende', 'deja cargador original', '18/11/2014', 2, 1, '16/11/2014', '03/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (14, 'se desarma el celular por completo y se hace baño quimico a la placa madre', '18/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (14, 'se rensambla el equipo y hace prueba, enciende pero falla microfono del equipo y la bateria esta casi muerta, se pide repuesto de microfono', '19/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (14, 'llega nuevo repuesto del microfono y se coloca al equipo, se puede hablar pero pesima calidad, se deja haciendo un nuevo baño quimico a la placa base', '24/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (14, 'se hace el segundo baño quimico con una limpieza profunda de conectores de la placa para sacar oxidacion o sarro, se prueba, el nuevo microfono funciona correctamente, pero la bateria se ve mas hinchada que la ultima vez, se cambia por una bateria nueva y se deja probando reproduciendo musica', '25/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (14, 'se hacen ultimas pruebas de rutina, todas las funciones ok, trabajo fuinalizado', '26/11/2014', 5);
--ORDEN 15
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (21, 'no agarra wifi y anda lenta', 'deja cargador original y funda color celeste', '19/11/2014', 2, 1, '21/11/2014', '24/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (15, 'se inspecciona la maquina y el interruptor de wifi esta desactivado, al activarse se pudo conectar a la red del lugar, lo unico raro es que al reiniciarse la maquina se debe encender de vuelta el equipo para poder conectarse, se comienza a optimizar el equipo', '19/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (15, 'se configuro analisis antivirus despues de la desfragmentacion del disco, se nota una mejora en la velocidad del sistema', '19/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (15, 'se termino el analisis antivirus y elimino unos 25 virus diferentes, se hace pruebas de estabilidad por el resto del dia', '20/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (15, 'se terminaron las pruebas de estabilidad del sistema, todas las funcionalidades ok, ahora el wifi se activa automaticamente, trabajo finalizado', '21/11/2014', 5);
--ORDEN 16
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (22, 'el usuario intento colocar una rom modificada y ahora no inicia el sistema en absoluto', 'el usuario tiene una backup del equipo, no es necesario tareas de backup', '20/11/2014', 2, 1, '26/11/2014', '26/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (16, 'se intenta restaurar el SO con la herramienta del fabricante, no se puede, el software no reconoce el equipo', '20/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (16, 'se intenta restaurar el SO con 2 herramientas alternativas, ninguna puede reconocer el equipo', '21/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (16, 'buscando en internet se intenta un hard-reset para limpiar por completo el telefono y que se pueda restaurar, no se puede lograr nada, se sigue buscando una solucion', '23/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (16, 'se inspecciona la placa del equipo para buscar fallas, se intento la conexion con la pc usando varios cables usb y ninguno funciono, por lo que se cambia conector micro-usb del equipo y se hace baño quimico', '25/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (16, 'se conecta el equipo al SO del fabricante y se lo reconocio, se pudo realizar la restauracion del SO a nivel de fabrica, se prueban funcionalidades y se deja haciendo prueba de estabilidad del sistema', '25/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (16, 'el equipo tiene todas las funcionalidades ok, trabajo finalizado', '26/11/2014', 5);
--ORDEN 17
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (23, 'dice que el sistema esta lento y con virus', 'deja cargador original, en caso de formatear quiere widows 7', '21/11/2014', 2, 1, '24/11/2014', '26/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (17, 'se hace inspeccion del sistema, el antivirus es un eset nod32 version 3, se hace un formateo con backup directamente', '21/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (17, 'se hace prueba de disco y memoria para sacar dudas, todo ok', '21/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (17, 'se hace reinstalacion de sistema y se restaura los datos recuperados, se deja el resto del dia para pruebas de estabilidad del sistama', '22/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (17, 'el equipo paso las pruebas de estabilidad de sistema, todas las funcionalidades ok, trabajo finalizado', '24/11/2014', 5);
--ORDEN 18
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (24, 'se cayo al piso y tiene pantalla partida, no se puede ver', 'ninguno', '22/11/2014', 2, 1, '27/11/2014', '02/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (18, 'se pide repuesto de pantalla con tactil incluido (solo viene los 2 juntos)', '22/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (18, 'llega al repuesto y se ensambla al equipo, se prueban funciones tactiles y se deja un par de horas para ver condicion del equipo', '26/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (18, 'se prueba el equipo por ultima vez, funcionalidades ok, tactil ok, trabajo terminado', '27/11/2014', 5);
--ORDEN 19
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (25, 'sistema lento y no se puede conectar a wifi pero si al 3g', 'ninguno', '24/11/2014', 3, 1, '26/11/2014', '29/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (19, 'se hace backup de documentos y contactos y se hace restauracion del SO desde el equipo mismo', '24/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (19, 'se hace restauracion del backup, instalacion de aplicaciones pedidas por el cliente y se deja para pruebas de estabilidad del sistema', '24/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (19, 'se prueba por ultima vez el equipo, funcionalidades ok, trabajo finalizado', '26/11/2014', 5);
--ORDEN 20
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (26, 'no se puede ingresar al sistema, apago el equipo antes cuando se hacian actualizaciones automaticas', 'deja cargador original y funda color verde con violeta', '25/11/2014', 3, 1, '29/11/2014', '01/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (20, 'se intenta restaurar sistema, las tenia desactivadas (no hay ninguna)', '25/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (20, 'se intenta restaurar el sistema usando cd de instalacion y por usb, no funciona ninguno de los metodos, se avisa al cliente que se debe formatear', '26/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (20, 'se acepta formateo con windows 8.1 (el mismo de fabrica) haciendo un previo backup de datos', '28/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (20, 'se instala el nuevo sistema y restaura el backup del usuario, se deja el resto del dia para pruebas de estabilidad', '28/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (20, 'se prueba el equipo por ultima vez, todas las funcionalidades ok, trabajo finalizado', '29/11/2014', 5);
--ORDEN 21
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (27, 'solo puede cargar la bateria hasta el 10%', 'deja cargador alternativo (el anterior se le rompio)', '26/11/2014', 2, 1, '28/11/2014', '29/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (21, 'se intenta carga con un cargador motorola original y por medio de usb, sigue cargandose hasta el 10%', '26/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (21, 'se inspecciona la bateria y no parece hinchada, aun asi se prueba con otra original de motorola y cargadores varios, sigue cargandose hasta el 10%', '26/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (21, 'se restaura el SO a nivel de fabrica (con previo backup de contactos y datos) usando la bateria original del equipo, se soluciona el problema, se deja actualizando el SO', '27/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (21, 'SO actualizado a ultima version posible y backup restaurado, el equipo carga normalmente la bateria y funciona ok, trabajo finalizado', '28/11/2014', 4);
--ORDEN 22
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (2, 'se le apaga el equipo despues de jugar 5 minutos a juegos de facebook y 30 minutos despues de usarla normalmente', 'ninugno', '27/11/2014', 2, 1, '29/11/2014', '03/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (22, 'se inspecciona la temperatura del equipo, inicia en 45 y a los 15 minutos roza los 90, se inspecciona el interior del equipo, demasiado polvo', '27/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (22, 'se hace limpieza de equipo a fondo, se cambia pasta termica del procesador y se deja por un dia en pruebas de estabilidad de sistema', '28/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (22, 'resumen del analisis del sistema, inicia con 30 y depues del dia entero solo llega a 65 grados con una media de 45, apta para funcionar, trabajo finalizado', '29/11/2014', 5);
--ORDEN 23
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (4, 'no funcionan los puertos usb', 'deja cargador original y funda color negra, la lectora de dvd no funciona pero no quiere repararla', '28/11/2014', 2, 1, '10/11/2014', '12/11/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (23, 'se inspecciona el SO e intenta conectar pendrives varios por todos los puertos, no reconoce ninguno, se actualizan drivers usb del equipo por internet y se intenta nuevamente, sigue sin poder reconocerlos', '28/11/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (23, 'se intenta conectar pendrive y lectora usb al equipo para cargar una distribucion linux, el bios no lo reconoce directamente', '28/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (23, 'se avisa al cliente que se debe mandar el equipo a un taller de reballing para saber mas profundamente el problema, cliente acepta', '29/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (23, 'el equipo de reballing avisa que el equipo tiene una falla en el puente sur del equipo y realizan cotizacion, cliente acepta', '9/12/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (23, 'el equipo reparado es devuelto del servicio tecnico especializado y se prueba, los usb ok, el bios y el SO lo reconocen, trabajo terminao', '10/12/2014', 5);
--ORDEN 24
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (6, 'al llamar escucha muy bajo, igual con la musica', 'deja cargador original', '29/11/2014', 3, 1, '06/12/2014', '11/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (24, 'tiene el volumen al maximo, se pide repuesto de parlante', '29/11/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (24, 'llego el parlante del equipo y se lo ensambla, se hacen pruebas con musica por el resto del dia', '3/12/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (24, 'pasado los 30 minutos de la prueba de muscia se corta, no se escucha sonidos en llamadas pero si por auricular, el repuesto estaba defectuoso, se pide un segundo repuesto', '03/12/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (24, 'llego el segundo repuesto y se instala en el equipo, se hace prueba de sonido con musica por un par de horas, sonido y otras funcionalidades ok, trabajo terminado', '06/12/2014', 4);
--ORDEN 25
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (8, 'se quemo la salida de auricular del equipo', 'ninguno', '01/12/2014', 2, 1, '03/12/2014', '06/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (25, 'se desarma el equipo e inspecciona la placa para ver daño alrededor de la entrada de auricular, por suerte no tiene daños alrededor, se pide repuesto', '01/12/2014',4 );
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (25, 'se consiguio el repuesto pedido y se procede a desoldar la parte dañada y soldar la nueva. La soldadura fue exitosa y se probo con auriculares por 30 minutos seguidos, sonido ok, trabajo finalizado', '03/12/2014', 4);
--ORDEN 26
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (10, 'se le cayo al piso y le aparece pantalla en negro con letras blanca, no inicia SO', 'deja cargador original y funda color verde fluor', '02/12/2014', 3, 1, '03/12/2014', '03/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (26, 'se inspecciona el equipo al encender, la pantalla ok, sin problemas, el bios no reconoce ningun disco, se desmonta para probarlo en otra parte', '02/12/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (26, 'se prueba el disco dañado en una pc de escritorio y un hub usb como disco externo, no lo reconoce el bios ni el SO, esta totalmente dañado, no se puede hacer recuperacion a menos que se lleve a un taller especializado, se envia presupuesto de disco nuevo con SO instalado', '02/12/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (26, 'el cliente no acepta presupuesto y se ensambla el equipo con el disco dañado, orden sin solucion', '03/12/2014', 4);
--ORDEN 27
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (12, 'no le anda el internet por cable', 'deja cargador y funda con motivos de green day', '03/12/2014', 2, 1, '03/12/2014', '04/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (27, 'se inspecciona el equipo y se intenta conectar por la red del negocio, no hay suerte, el driver esta, pero no conecta', '03/12/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (27, 'se reinstala driver desde la pagina del fabricante y se prueba la conexion, funciona y se deja en pruebas de estabilidad por el resto del dia', '03/12/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (27, 'el equipo no se ha desconectado en nigun momento de las 4 horas que se conecto, trabajo finalizado', '03/12/2014', 5);
--ORDEN 28
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (3, 'se cayo una cerveza al equipo y el teclado no funciona', 'deja cargador original', '04/12/2014', 2, 1, '06/12/2014', '09/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (28, 'se desarma el equipo y se limpia la placa con alcohol isopropilico, a su vez el teclado tambien se limpia, se deja secar por el resto de la noche', '04/12/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (28, 'se ensambla el equipo entrero y se prueba, funciona solo 5 teclas del teclado entero, se pide repuesto del mismo', '05/12/2014', 4);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (28, 'llego el repuesto de teclado y se ensambla al equipom todas las teclas funcionan, trabajo terminado', '09/12/2014', 4);
--ORDEN 29
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (5, 'no enciende despues de un corte de luz el dia anterior', 'ninguno', '05/12/2014', 3, 1, '06/12/2014', '10/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (29, 'se conecta el equipo al taller y no enciende, se inspecciona el interior del mismo y no parece tener quemaduras visibles. se mide fuente de alimentacion y no enciende en absoluto, por el momento se necesita de una fuente para hacer pruebas', '05/12/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (29, 'se consigue la fuente de prueba y se ensambla el equipo, enciende sin problemas, el bios muestra que las tensiones son normales, se deja encendido con pruebas de estabilidad de sistema', '06/12/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (29, 'el equipo no se apago en ningun momento de las pruebas, esta ok, trabajo finalizado', '06/12/2014', 5);
--ORDEN 30
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (7, 'se apaga al estar usandose por 20 minutos', 'deja cargador original, no es usada para juegos', '06/12/2014', 2, 1, '10/12/2014', '13/12/2014');
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (30, 'se inspecciona la temperatura del equipo por unos 20 minutos, commienza con 45 y llega a los 95, se nota el calor al tacto, es necesario una limpieza de hardware', '06/12/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (30, 'se desarma el equipo para llegar al sistema de ventilacion, se desarma dicho sistema para sacar el polvo y limpiarlo, se vuelve a ensamblar y se lo deja ejecutando pruebas de estabilidad del sistema', '09/12/2014', 5);
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (30, 'se inspecciona el equipo por ultima vez y las temperaturas no superan los 50 grados con una temperatura media de 40, se pruebas todas las funcionalidades, todo ok, trabajo finalizado', '10/12/2014', 4);
--ORDEN 30
INSERT INTO Ordenes(idEquipo, falla, observ, fecRecib, idEstado, aviso, fecAviso, fecEntrega) VALUES (11, 'se quebro la pantalla', 'deja cargador original, no es usada para juegos', getdate(), 1, 0, null, null);

/*
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (, '', '', );
INSERT INTO Tareas(idOrden, detalles, fecTarea, idUsu) VALUES (, '', '', );
*/

CREATE TRIGGER eliminarBarrios
on Barrios
INSTEAD OF DELETE
AS
BEGIN
	DECLARE @barrio smallint
	SELECT @barrio = idBarrio FROM DELETED
		
	UPDATE Usuarios SET idBarrio = 1 WHERE idBarrio = @barrio;
	UPDATE Clientes SET idBarrio = 1 WHERE idBarrio = @barrio;

	DELETE FROM Barrios WHERE idBarrio = @barrio;
END

CREATE TRIGGER eliminarTipoMarca
on TipoMarca
INSTEAD OF DELETE
AS
BEGIN
	DECLARE @tipoMarca smallint
	SELECT @tipoMarca = idTipoMarca FROM DELETED
	
	UPDATE Equipos SET idTipoMarca = 1 WHERE idTipoMarca = @tipoMarca;
	
	DELETE FROM TipoMarca WHERE idTipoMarca = @tipoMarca;
END